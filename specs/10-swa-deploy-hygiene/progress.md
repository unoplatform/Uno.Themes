# Azure Static Web Apps deploy hygiene

Follow-up to the investigation of "the deployed app is still the very old one".

## Findings

The report was that `.github/workflows/azure-static-web-apps.yml` fails to upload new builds
on `master`. It does not — **production is current and the master leg is healthy**:

- Last `master` push: `47d42c18` (merge of #1715), 2026-09-04. Run
  [33909348684](https://github.com/unoplatform/Uno.Themes/actions/runs/33909348684) reported
  `Status: Succeeded` / `Deployment Complete :)` for
  `https://salmon-rock-0cfebe70f.azurestaticapps.net`.
- The live site's `Last-Modified` is `Fri, 04 Sep 2026 19:13:44 GMT` — the minute that deploy
  finished — and `origin/master` is still `47d42c18`, so there is no newer build to upload.
- The live `manifest.webmanifest` is `ThemesSampleApp` ("Uno.Themes Samples"), and its
  `uno-config.js` lists the `GuestApps/{Material,Cupertino,Simple}SampleApp/` payloads, so the
  ALC wrapper head (#1693) is what is deployed.
- The deployed `GuestApps/SimpleSampleApp/Uno.Themes.WinUI.dll.bin` contains `DefaultSpacing`,
  `DefaultFontFamily`, `Density`, `DefaultCornerRadius` and `GetTheme` — the #1699 / #1701 /
  #1707 / #1715 work.
- Stale service-worker caching was ruled out: the published `service-worker.js` is network-first
  (it only falls back to cache when `fetch` throws).

What is actually broken is the **pull-request preview** leg. Every PR run since at least
2026-08-20 fails in the `deploy` job with:

```
The content server has rejected the request with: BadRequest
Reason: This Static Web App already has the maximum number of staging environments
(System.Threading.Tasks.Task`1[System.Int32]). Please remove one and try again.
```

(The handful of PR runs that report success are ~29s `close-pull-request` runs fired when a PR
merges, not deploys.) So a PR never gets a preview URL, and checking the production URL instead
shows master's build — which reads as "old".

### Capacity, not only leakage

Azure caps staging environments per site: **3** on the Free plan, **10** on Standard. The repo
currently has **12 open pull requests**, each of which wants one. Reclaiming orphans is
necessary but, on its own, cannot be sufficient at that ratio — see "Not done here" below.

## Changes

- [x] `.github/workflows/azure-static-web-apps.yml`: drop `production_branch: master`.
      `Azure/static-web-apps-deploy@v1` does not declare that input and logs
      `##[warning]Unexpected input(s) 'production_branch'`; it was silently ignored. The action
      already derives the target from the event (push → production, pull_request → staging), so
      behaviour is unchanged and the warning goes away.
- [x] `src/samples/ThemesSampleApp/Platforms/WebAssembly/wwwroot/staticwebapp.config.json`:
      added. The three theme heads each ship one, but the wrapper head — the project actually
      deployed since #1693 — did not, so the site ran on Static Web Apps defaults. Verified
      against production: `/some/deep/link` returns **404** (no `navigationFallback`) and
      `/package_*` assets are served `max-age=30` instead of the intended
      `immutable, max-age=31536000` on a ~51 MB payload.
      The file matches the other three heads except for one deliberate difference: `bin` is
      added to the `navigationFallback.exclude` globs. `GuestAppLoader.Wasm` fetches the guest
      payload as `GuestApps/<App>/<Assembly>.dll.bin`; without the exclusion a missing payload
      file would be rewritten to `index.html` and returned `200`, and the loader would write
      HTML into `<Assembly>.dll` and fail later with an opaque `BadImageFormatException`
      instead of a clean not-found.
- [x] `.github/workflows/azure-static-web-apps-prune.yml`: added. Reconciles the site's staging
      environments against the open pull requests daily (06:00 UTC) and on demand, deleting the
      orphans. `close-pull-request` only reclaims an environment when that job actually runs —
      PRs closed before it existed, cancelled runs, and any failure of the step all strand one
      with nothing to collect it.

### Prune workflow notes

- Skips the production environment: `az staticwebapp environment list` reports it as
  `buildId: "default"`; every other `buildId` is the PR number.
- A `workflow_dispatch` run defaults to **dry run** (reports orphans to the step summary,
  deletes nothing). Scheduled runs always delete.
- One failed delete does not strand the environments queued behind it; the step warns per
  failure and exits non-zero at the end.
- The job **no-ops with a `::notice`** when the repository is not configured, so it never turns
  into a nightly red X on a fork or before the secrets exist. Required configuration:

  | Kind | Name | Purpose |
  | ---- | ---- | ------- |
  | variable | `AZURE_SWA_NAME` | Static Web App resource name |
  | variable | `AZURE_SWA_RESOURCE_GROUP` | its resource group |
  | secret | `AZURE_CLIENT_ID` | federated (OIDC) app registration, Contributor on the site |
  | secret | `AZURE_TENANT_ID` | |
  | secret | `AZURE_SUBSCRIPTION_ID` | |

## Verification

- Both workflow files parse as YAML; job and step graphs unchanged apart from the additions.
- `staticwebapp.config.json` validated with `json.tool`; byte-for-byte identical to the three
  theme heads' copies apart from the `bin` exclusion (LF, 2-space, trailing newline).
- The prune step's shell body was extracted from the workflow and exercised against stubbed
  `az` / `gh` for four cases:
  - delete mode with 7 environments vs 5 open PRs → keeps `default` and every open PR, deletes
    both orphans, survives a mid-loop delete failure (warns, continues, exits 1);
  - dry run → reports the orphans, deletes nothing, exits 0;
  - no orphans → clean exit 0;
  - configuration guard → `configured=false` + notice when unset, `configured=true` when set.

## Not done here

- **Reclaiming the currently-full quota.** Deleting the existing staging environments needs
  Azure portal / CLI access against the subscription and cannot be done from the repo. Until
  that happens (or the prune job runs once configured) PR previews keep failing.
- **Capping demand.** With 12 open PRs and a maximum of 10 environments, previews cannot be
  guaranteed for every PR even with perfect cleanup. The durable fix is to make previews
  opt-in — gate the `build`/`deploy` jobs on a label (e.g. `preview`) so routine PRs never
  consume a slot. That changes the workflow for every contributor, so it is left as a team
  decision rather than folded into this change.
