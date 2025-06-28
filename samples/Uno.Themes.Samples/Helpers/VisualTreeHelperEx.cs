using System.Text;
using Uno.UI.Extensions;

namespace Uno.Themes.Samples.Helpers;

public static partial class VisualTreeHelperEx // debugging
{

	public static void MonitorVisualStates(this Control control, bool includeInitialStates, Action<string> onStatesChanged)
	{
		var root = (FrameworkElement)control.EnumerateDescendants().FirstOrDefault();
		if (root == null)
		{
			onStatesChanged("The control doesn't have a template root.");
			return;
		}
		var vsgs = VisualStateManager.GetVisualStateGroups(root).ToArray();
		if (!vsgs.Any())
		{
			onStatesChanged("The control template doesn't contain any VisualStateGroup.");
			return;
		}

		foreach (var vsg in vsgs)
		{
			vsg.CurrentStateChanged += (s, e) => DebugVStates(vsg.Name);
		}
		if (includeInitialStates)
		{
			DebugVStates();
		}

		void DebugVStates(string updatedGroupName = null)
		{
			var summary = string.Join("\n", vsgs
				.Where(x => x.CurrentState != null)
				.Select(x => $"{x.Name}: {x.CurrentState?.Name}")
			);
			onStatesChanged(summary);
		}
	}
}
