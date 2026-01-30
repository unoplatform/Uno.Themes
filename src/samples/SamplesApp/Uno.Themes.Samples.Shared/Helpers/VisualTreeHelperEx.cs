using System.Text;

#if __IOS__
using UIKit;
using _View = UIKit.UIView;
#elif __MACOS__
using AppKit;
using _View = AppKit.NSView;
#elif __ANDROID__
using _View = Android.Views.View;
#elif IS_WINUI
using _View = Microsoft.UI.Xaml.DependencyObject;
#else
using _View = Windows.UI.Xaml.DependencyObject;
#endif

namespace Uno.Themes.Samples.Helpers;

public static partial class VisualTreeHelperEx
{
	/// <summary>
	/// Returns the first ancestor of a specified type.
	/// </summary>
	/// <typeparam name="T">The type of ancestor to find.</typeparam>
	/// <param name="reference">Any node of the visual tree</param>
	/// <remarks>First Counting from the <paramref name="reference"/> and not from the root of tree.</remarks>
	public static T FindFirstAncestor<T>(this _View reference) => EnumerateAncestors(reference)
		.OfType<T>()
		.FirstOrDefault();

	/// <summary>
	/// Returns the first ancestor of a specified type that satisfies the <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="T">The type of ancestor to find.</typeparam>
	/// <param name="reference">Any node of the visual tree</param>
	/// <param name="predicate">A function to test each node for a condition.</param>
	/// <remarks>First Counting from the <paramref name="reference"/> and not from the root of tree.</remarks>
	public static T FindFirstAncestor<T>(this _View reference, Func<T, bool> predicate) => EnumerateAncestors(reference)
		.OfType<T>()
		.FirstOrDefault(predicate);

	/// <summary>
	/// Returns the first descendant of a specified type.
	/// </summary>
	/// <typeparam name="T">The type of descendant to find.</typeparam>
	/// <param name="reference">Any node of the visual tree</param>
	/// <remarks>The crawling is done depth first.</remarks>
	public static T FindFirstDescendant<T>(this _View reference) => EnumerateDescendants(reference)
		.OfType<T>()
		.FirstOrDefault();

	/// <summary>
	/// Returns the first descendant of a specified type that satisfies the <paramref name="predicate"/>.
	/// </summary>
	/// <typeparam name="T">The type of descendant to find.</typeparam>
	/// <param name="reference">Any node of the visual tree</param>
	/// <param name="predicate">A function to test each node for a condition.</param>
	/// <remarks>The crawling is done depth first.</remarks>
	public static T FindFirstDescendant<T>(this _View reference, Func<T, bool> predicate) => EnumerateDescendants(reference)
		.OfType<T>()
		.FirstOrDefault(predicate);

	/// <summary>
	/// Returns the first descendant of a specified type that satisfies the <paramref name="predicate"/> whose ancestors (up to <paramref name="reference"/>) and itself satisfy the <paramref name="hierarchyPredicate"/>.
	/// </summary>
	/// <typeparam name="T">The type of descendant to find.</typeparam>
	/// <param name="reference">Any node of the visual tree</param>
	/// <param name="hierarchyPredicate">A function to test each ancestor for a condition.</param>
	/// <param name="predicate">A function to test each descendant for a condition.</param>
	/// <remarks>The crawling is done depth first.</remarks>
	public static T FindFirstDescendant<T>(this _View reference, Func<_View, bool> hierarchyPredicate, Func<T, bool> predicate) => EnumerateDescendants(reference, hierarchyPredicate)
		.OfType<T>()
		.FirstOrDefault(predicate);

	/// <summary>
	/// Returns all the visual descendants of a node.
	/// </summary>
	/// <param name="reference">Any node of the visual tree</param>
	public static IEnumerable<_View> EnumerateDescendants(this _View reference) => EnumerateDescendants(reference, x => true);

	/// <summary>
	/// Returns all the visual descendants of a node that satisfies the <paramref name="hierarchyPredicate"/>.
	/// </summary>
	/// <param name="reference">Any node of the visual tree</param>
	/// <param name="hierarchyPredicate"></param>
	/// <returns></returns>
	public static IEnumerable<_View> EnumerateDescendants(this _View reference, Func<_View, bool> hierarchyPredicate)
	{
		foreach (var child in reference.EnumerateChildren().Where(hierarchyPredicate))
		{
			yield return child;
			foreach (var grandchild in child.EnumerateDescendants(hierarchyPredicate))
			{
				yield return grandchild;
			}
		}
	}
	

	// note: methods for retrieving children/ancestors exist with varying signatures.
	// re-implementing them with unified & more inclusive signature for convenience.
#if __IOS__ || __MACOS__
	private static IEnumerable<_View> EnumerateAncestors(this _View o)
	{
		if (o is null) yield break;
		while (o.Superview is _View parent)
		{
			yield return o = parent;
		}
	}

	private static IEnumerable<_View> EnumerateChildren(this _View o)
	{
		if (o is null) return Enumerable.Empty<_View>();
		return o.Subviews;
	}
#elif __ANDROID__
	private static IEnumerable<_View> EnumerateAncestors(this _View o)
	{
		if (o is null) yield break;

		while (o.Parent is _View parent)
		{
			yield return o = parent;
		}
	}

	private static IEnumerable<_View> EnumerateChildren(this _View reference)
	{
		if (reference is Android.Views.ViewGroup vg)
		{
			return Enumerable
				.Range(0, vg.ChildCount)
				.Select(vg.GetChildAt)
				.Where(x => x != null).Cast<_View>();
		}

		return Enumerable.Empty<_View>();
	}
#else
	private static IEnumerable<_View> EnumerateAncestors(this _View o)
	{
		if (o is null) yield break;
		while (VisualTreeHelper.GetParent(o) is DependencyObject parent)
		{
			yield return o = parent;
		}
	}

	private static IEnumerable<_View> EnumerateChildren(this _View reference)
	{
		return Enumerable
			.Range(0, VisualTreeHelper.GetChildrenCount(reference))
			.Select(x => VisualTreeHelper.GetChild(reference, x));
	}
#endif

	public static _View GetTemplateRoot(this Control control)
	{
		return EnumerateChildren(control).FirstOrDefault();
	}
}
public static partial class VisualTreeHelperEx // debugging
{
	/// <summary>
	/// Produces a text representation of the visual tree.
	/// </summary>
	/// <param name="reference">Any node of the visual tree</param>
	public static string TreeGraph(this _View reference) => TreeGraph(reference, DescribeVTNode);

	/// <summary>
	/// Produces a text representation of the visual tree, using the provided method of description.
	/// </summary>
	/// <param name="reference">Any node of the visual tree</param>
	/// <param name="describe">A function to describe a visual tree node in a single line.</param>
	/// <returns></returns>
	public static string TreeGraph(this _View reference, Func<_View, string> describe)
	{
		var buffer = new StringBuilder();
		try
		{
			Walk(reference);
		}
		catch (Exception e)
		{
			buffer.AppendLine();
			buffer.AppendLine("An error has occurred while walking the visual tree:");
			buffer.Append(e.ToString());
		}
		return buffer.ToString();

		void Walk(_View o, int depth = 0)
		{
			Print(o, depth);
			foreach (var child in EnumerateChildren(o))
			{
				Walk(child, depth + 1);
			}
		}
		void Print(_View o, int depth)
		{
			buffer
				.Append(new string(' ', depth * 4))
				.Append(describe(o))
				.AppendLine();
		}
	}

	public static void MonitorVisualStates(this Control control, bool includeInitialStates, Action<string> onStatesChanged)
	{
		var root = (FrameworkElement)control.GetTemplateRoot();
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

	private static string DescribeVTNode(object x)
	{
		return new StringBuilder()
			.Append(x.GetType().Name)
			.Append((x as FrameworkElement)?.Name is string xname && xname.Length > 0 ? $"#{xname}" : string.Empty)
			.Append($" // {string.Join(", ", GetDetails())}")
			.ToString();

		bool TryGetDpValue<T>(object owner, string property, out T value)
		{
			if (owner is DependencyObject @do &&
				owner.GetType().GetProperty($"{property}Property", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.GetValue(null, null) is DependencyProperty dp)
			{
				value = (T)@do.GetValue(dp);
				return true;
			}

			value = default;
			return false;
		}
		string FormatCornerRadius(CornerRadius cr)
		{
			// format: uniform, [left,top,right,bottom]
			if (cr.TopLeft == cr.TopRight && cr.TopRight == cr.BottomRight && cr.BottomRight == cr.BottomLeft) return $"{cr.TopLeft}";
			return $"[{cr.TopLeft},{cr.TopRight},{cr.BottomRight},{cr.BottomLeft}]";
		}
		string FormatThickness(Thickness thickness)
		{
			// format: uniform, [same-left-right,same-top-bottom], [left,top,right,bottom]
			if (thickness.Left == thickness.Top && thickness.Top == thickness.Right && thickness.Right == thickness.Bottom) return $"{thickness.Left}";
			if (thickness.Left == thickness.Right && thickness.Top == thickness.Bottom) return $"[{thickness.Left},{thickness.Top}]";
			return $"[{thickness.Left},{thickness.Top},{thickness.Right},{thickness.Bottom}]";
		}
		string FormatLengthRange(double min, double value, double max)
		{
			if (min == 0 && max == double.PositiveInfinity) return value.ToString();

			return $"[{min},{value},{max}]";
		}
		string FormatGridLength(GridLength value)
		{
			// format: A,*,2.5*,2.5
			if (value.IsAuto) return "A";
			if (value.IsStar) return value.Value == 1 ? "*" : (value.Value.ToString() + "*");
			return value.Value.ToString();
		}
		IEnumerable<string> GetDetails()
		{
			if (x is FrameworkElement fe)
			{
				yield return $"Actual={fe.ActualWidth}x{fe.ActualHeight}";
				//if (fe.TransformToVisual(Window.Current.Content).TransformPoint(default) is var absPos)
				//{
				//	yield return $"AbsPos={absPos.X},{absPos.Y}";
				//}
				yield return $"HV={fe.HorizontalAlignment}/{fe.VerticalAlignment}";
			}
			if (x is Grid grid)
			{
				if (grid.ColumnDefinitions.Any()) yield return $"Columns='{string.Join(',', grid.ColumnDefinitions.Select(cd => FormatGridLength(cd.Width)))}'";
				if (grid.RowDefinitions.Any()) yield return $"Rows='{string.Join(',', grid.RowDefinitions.Select(rd => FormatGridLength(rd.Height)))}'";
			}
			/*if (x is FrameworkElement fe2 &&
				Grid.GetRow(fe2) is var row &&
				Grid.GetRowSpan(fe2) is var rowSpan &&
				Grid.GetColumn(fe2) is var column &&
				(row > 0 || rowSpan > 1 || ...)
				)
			{
				yield return $"R{
			}*/
			if (TryGetDpValue<Thickness>(x, "Margin", out var margin)) yield return $"Margin={FormatThickness(margin)}";
			if (TryGetDpValue<Thickness>(x, "Padding", out var padding)) yield return $"Padding={FormatThickness(padding)}";
			if (TryGetDpValue<CornerRadius>(x, "CornerRadius", out var cr)) yield return $"CornerRadius={FormatCornerRadius(cr)}";
			if (TryGetDpValue<Thickness>(x, "BorderThickness", out var bt)) yield return $"BorderThickness={FormatThickness(bt)}";

			if (TryGetDpValue<double>(x, "MinWidth", out var minWidth) &&
				TryGetDpValue<double>(x, "Width", out var width) &&
				TryGetDpValue<double>(x, "MaxWidth", out var maxWidth) &&
				TryGetDpValue<double>(x, "MinHeight", out var minHeight) &&
				TryGetDpValue<double>(x, "Height", out var height) &&
				TryGetDpValue<double>(x, "MaxHeight", out var maxHeight)) yield return $"Size={FormatLengthRange(minWidth, width, maxWidth)}x{FormatLengthRange(minHeight, height, maxHeight)}";

			if (TryGetDpValue<double>(x, "Opacity", out var opacity)) yield return $"Opacity={opacity}";
			if (TryGetDpValue<Visibility>(x, "Visibility", out var visibility)) yield return $"Visibility={visibility}";
			if (x is Control c && 
				c.GetTemplateRoot() is FrameworkElement templateRoot &&
				VisualStateManager.GetVisualStateGroups(templateRoot) is var vsgs &&
				vsgs.Count > 0)
			{
				//yield return "VisualStates=" + string.Join("|", vsgs.Where(y => y.CurrentState != null).Select(y => $"{y.Name}={y.CurrentState.Name}"));
				yield return "VisualStates=" + string.Join("|", vsgs.Where(y => y.CurrentState != null).Select(y => y.CurrentState.Name));
			}
		}
	}
}
