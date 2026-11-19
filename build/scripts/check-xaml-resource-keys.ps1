<#
.SYNOPSIS
	Fails when theme or sample XAML references a resource key that nothing defines.

.DESCRIPTION
	An undefined {StaticResource}/{ThemeResource} key is a bug on every platform: Uno silently drops it (the
	control renders unstyled), and WinUI fails the template, usually as a native fail-fast that takes a
	WinAppSDK app down the moment the control or page is realized. This check resolves every reference
	statically:

	- Library styles: Simple and Material v2 (and the shared Uno.Themes base) against their own keys, the shared
	  base styles and the WinUI platform keys.
	- Sample pages: each page against the keys of every head (design) that lists it, ignoring the
	  SamplePageLayout templates of the other designs, which that head never renders.

	Cupertino and Material v1 are out of scope for now.

	WinUI's own keys come from its generic.xaml, so a WinAppSDK head must have been restored first (CI runs this
	in the WinAppSDK job, after its build). Keys generated in code by BaseTheme (Space*, Radius*, ControlHeight*,
	IconSize*) and WinUI's System* keys are allowed by prefix.

.PARAMETER GenericXaml
	WinUI's generic.xaml. Defaults to the newest Microsoft.WindowsAppSDK.WinUI in the NuGet cache.
#>
param(
	[string]$RepoRoot = (Resolve-Path "$PSScriptRoot/../.."),
	[string]$GenericXaml
)

$ErrorActionPreference = 'Stop'
$xamlNs = 'http://schemas.microsoft.com/winfx/2006/xaml'

# Declared in code (BaseTheme scale generation) or by the WinUI platform outside generic.xaml.
$allowedPrefixes = 'Space', 'Radius', 'ControlHeight', 'IconSize', 'System'
$allowedKeys = @('ScrollViewerScrollBarlessTemplate')

if (-not $GenericXaml) {
	$cache = if ($env:NUGET_PACKAGES) { $env:NUGET_PACKAGES } else { Join-Path $HOME '.nuget/packages' }
	$GenericXaml = Get-ChildItem (Join-Path $cache 'microsoft.windowsappsdk.winui') -Directory -ErrorAction SilentlyContinue |
		Sort-Object { [version]($_.Name -replace '-.*$', '') } -Descending |
		ForEach-Object { Join-Path $_.FullName 'lib/native/Microsoft.UI/Themes/generic.xaml' } |
		Where-Object { Test-Path $_ } | Select-Object -First 1
	if (-not $GenericXaml) { throw 'WinUI generic.xaml not found in the NuGet cache; restore a WinAppSDK head first or pass -GenericXaml.' }
}
$platformKeys = [System.Collections.Generic.HashSet[string]]::new([string[]]([regex]::Matches((Get-Content $GenericXaml -Raw), 'x:Key="([^"]+)"') | ForEach-Object { $_.Groups[1].Value }))

$src = Join-Path $RepoRoot 'src'
$library = Join-Path $src 'library'
$samples = Join-Path $src 'samples'

function Get-XamlFiles([string]$Root, [string[]]$Exclude = @()) {
	Get-ChildItem $Root -Recurse -Filter *.xaml |
		Where-Object { $_.FullName -notmatch '[\\/](obj|bin|Generated)[\\/]' } |
		Where-Object { $path = $_.FullName; -not ($Exclude | Where-Object { $path -match $_ }) }
}

function Read-Xaml([string]$Path) {
	$settings = [System.Xml.XmlReaderSettings]::new()
	$settings.IgnoreComments = $true
	$reader = [System.Xml.XmlReader]::Create($Path, $settings)
	try { $doc = [System.Xml.XmlDocument]::new(); $doc.Load($reader); $doc } finally { $reader.Dispose() }
}

function Get-DefinedKeys([System.IO.FileInfo[]]$Files) {
	$keys = [System.Collections.Generic.HashSet[string]]::new()
	foreach ($file in $Files) {
		foreach ($attr in (Read-Xaml $file.FullName).SelectNodes('//@*')) {
			if ($attr.NamespaceURI -eq $xamlNs -and ($attr.LocalName -eq 'Key' -or $attr.LocalName -eq 'Name')) {
				[void]$keys.Add($attr.Value)
			}
		}
	}
	, $keys # the comma keeps PowerShell from unrolling the set into an array
}

function Get-ReferencedKeys([System.Xml.XmlNode]$Root) {
	$pattern = '\{(?:Static|Theme)Resource\s+(?:ResourceKey=)?([A-Za-z0-9_.]+)\s*\}'
	foreach ($attr in $Root.SelectNodes('.//@*')) {
		if ($attr.LocalName -eq 'ResourceKey' -and $attr.Value -notmatch '^\{') { $attr.Value }
		foreach ($match in [regex]::Matches($attr.Value, $pattern)) { $match.Groups[1].Value }
	}
}

function Test-Allowed([string]$Key) {
	($allowedKeys -contains $Key) -or ($allowedPrefixes | Where-Object { $Key.StartsWith($_) })
}

$failures = [System.Collections.Generic.List[string]]::new()

function Test-File([string]$Scope, [System.IO.FileInfo]$File, [System.Xml.XmlNode]$Root, $Defined) {
	$missing = Get-ReferencedKeys $Root | Sort-Object -Unique |
		Where-Object { -not $Defined.Contains($_) -and -not $platformKeys.Contains($_) -and -not (Test-Allowed $_) }
	if ($missing) {
		$relative = [System.IO.Path]::GetRelativePath($RepoRoot, $File.FullName) -replace '\\', '/'
		$failures.Add("[$Scope] ${relative}: $($missing -join ', ')")
	}
}

# Each theme as a consumer merges it: its own files and the shared base styles.
$baseFiles = @(Get-XamlFiles (Join-Path $library 'Uno.Themes/Styles'))
$themes = [ordered]@{
	'Themes'      = @{ Files = $baseFiles }
	'Simple'      = @{ Files = @(Get-XamlFiles (Join-Path $library 'Uno.Simple.WinUI/Styles')) }
	'Material v2' = @{ Files = @(Get-XamlFiles (Join-Path $library 'Uno.Material/Styles') -Exclude '[\\/]v1[\\/]') }
}
foreach ($theme in $themes.Values) { $theme.Defined = Get-DefinedKeys ($theme.Files + $baseFiles) }

foreach ($name in $themes.Keys) {
	foreach ($file in $themes[$name].Files) {
		if ($name -ne 'Themes' -and $baseFiles.FullName -contains $file.FullName) { continue }
		Test-File $name $file (Read-Xaml $file.FullName).DocumentElement $themes[$name].Defined
	}
}

# Sample heads: which theme they load, which SamplePageLayout templates they never render.
$heads = [ordered]@{
	'Material' = @{ Theme = 'Material v2'; Skip = 'MaterialTemplate', 'CupertinoTemplate', 'SimpleTemplate' }
	'Simple'   = @{ Theme = 'Simple'; Skip = 'MaterialTemplate', 'M3MaterialTemplate', 'CupertinoTemplate' }
}
$sharedFiles = @(Get-XamlFiles (Join-Path $samples 'SamplesApp.Shared'))
$sharedKeys = Get-DefinedKeys $sharedFiles

# Which designs realize a file. A [SamplePage] lists them. Other shared XAML (nested pages, user controls)
# inherits the designs of the sample pages that reference its class; anything referenced from elsewhere
# (the shell, app styles) or not at all is realized by every head. Runtime tests are not UI hosts.
$sourceFiles = Get-ChildItem $samples -Recurse -Include *.xaml, *.cs |
	Where-Object { $_.FullName -notmatch '[\\/](obj|bin|RuntimeTests)[\\/]' }
$sourceText = @{}
foreach ($file in $sourceFiles) { $sourceText[$file.FullName] = Get-Content $file.FullName -Raw }
$allDesigns = @('Material', 'Simple', 'Cupertino')

function Get-PageDesigns([string]$XamlPath) {
	$codeBehind = $sourceText["$XamlPath.cs"]
	if ($codeBehind -match '\[SamplePage\(') {
		return , @([regex]::Matches($codeBehind, 'Design\.(\w+)') | ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique)
	}
	return $null
}

function Get-Designs([System.IO.FileInfo]$File) {
	$page = Get-PageDesigns $File.FullName
	if ($null -ne $page) { return , $page }

	$class = if ($sourceText[$File.FullName] -match 'x:Class="[^"]*\.(\w+)"') { $Matches[1] }
	if (-not $class) { return , $allDesigns }

	$designs = [System.Collections.Generic.HashSet[string]]::new()
	foreach ($other in $sourceFiles) {
		if ($other.FullName -like "$($File.FullName)*" -or $sourceText[$other.FullName] -notmatch "\b$class\b") { continue }
		$hostDesigns = Get-PageDesigns ($other.FullName -replace '\.cs$', '')
		if ($null -eq $hostDesigns) { return , $allDesigns }
		foreach ($design in $hostDesigns) { [void]$designs.Add($design) }
	}
	if ($designs.Count) { return , @($designs) } else { return , $allDesigns }
}

$fileDesigns = @{}
foreach ($file in $sharedFiles) { $fileDesigns[$file.FullName] = Get-Designs $file }

foreach ($design in $heads.Keys) {
	$head = $heads[$design]
	$headFiles = @(Get-XamlFiles (Join-Path $samples "${design}SampleApp"))
	$defined = [System.Collections.Generic.HashSet[string]]::new($themes[$head.Theme].Defined)
	foreach ($key in $sharedKeys) { [void]$defined.Add($key) }
	foreach ($key in (Get-DefinedKeys $headFiles)) { [void]$defined.Add($key) }

	foreach ($file in $sharedFiles + $headFiles) {
		if ($fileDesigns.ContainsKey($file.FullName) -and $fileDesigns[$file.FullName] -notcontains $design) { continue }

		$doc = Read-Xaml $file.FullName
		foreach ($slot in $head.Skip) {
			foreach ($node in @($doc.SelectNodes("//*[local-name()='SamplePageLayout.$slot']"))) {
				[void]$node.ParentNode.RemoveChild($node)
			}
		}
		Test-File "$design head" $file $doc.DocumentElement $defined
	}
}

if ($failures.Count) {
	Write-Host "Undefined XAML resource keys ($($failures.Count) files). WinUI fails on these where Uno silently ignores them:"
	$failures | ForEach-Object { Write-Host "  $_" }
	exit 1
}

Write-Host 'All XAML resource references resolve.'
