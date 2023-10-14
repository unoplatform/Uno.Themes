using System.Xml.Linq;
using static Uno.Markup.Xaml.Helpers.ValueSimplifier;

namespace Uno.Markup.Xaml.UI.Xaml.Media.Animation;

public record Timeline(string Type)
{
	public string? TargetName { get; private set; }
	public string? TargetProperty { get; private set; }
	public string? Value { get; private set; }
	//public string? ValueTimeline { get; private set; }

	public static Timeline? Parse(XElement e)
	{
		return e.GetNameParts() switch
		{
			(_, "ColorAnimation") => ParseSimpleAnimation(),
			(_, "ColorAnimationUsingKeyFrames") => ParseKeyFrames(),
			(_, "DoubleAnimation") => ParseSimpleAnimation(),
			(_, "DoubleAnimationUsingKeyFrames") => ParseKeyFrames(),
			(_, "DragItemThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "DragOverThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "DrillInThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "DrillOutThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "DropTargetItemThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "FadeInThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "FadeOutThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "ObjectAnimationUsingKeyFrames") => ParseKeyFrames(),
			(_, "PointAnimation") => ParseSimpleAnimation(),
			(_, "PointAnimationUsingKeyFrames") => ParseKeyFrames(),
			(_, "PointerDownThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "PointerUpThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "PopInThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "PopOutThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "RepositionThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "SplitCloseThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "SplitOpenThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "SwipeBackThemeAnimation") => ParsePreconfiguredAnimation(),
			(_, "SwipeHintThemeAnimation") => ParsePreconfiguredAnimation(),

			_ => null,
		};

		Timeline ParseCommon()
		{
			var result = new Timeline(e.Name.LocalName);
			result.TargetName = e.Attribute("Storyboard.TargetName")?.Value;
			result.TargetProperty = e.Attribute("Storyboard.TargetProperty")?.Value;

			return result;
		}
		Timeline ParseKeyFrames()
		{
			var result = ParseCommon();
			result.Value = string.Join("\n", e.Elements().Select(ParseKeyFrame));

			return result;
		}
		string ParseKeyFrame(XElement frame)
		{
			string Value(string key = "Value") => SimplifyMarkup(frame.Attribute(key)?.Value ?? frame.GetMember("Value").Value);
			string KeyTime(string key = "KeyTime") => SimplifyMarkup(SimplifyTimeSpan(frame.Attribute(key)?.Value));
			string Raw(string key) => frame.Attribute(key)?.Value;

			return frame.Name.LocalName switch
			{
				"DiscreteObjectKeyFrame" => $"{Value()} @{KeyTime()}",

				// DoubleKeyFrame
				"DiscreteDoubleKeyFrame" => $"{Value()} @{KeyTime()}",
				"EasingDoubleKeyFrame" => $"{Value()} @{KeyTime()} f={frame.Attribute("EasingFunction")}",
				"LinearDoubleKeyFrame" => $"{Value()} @{KeyTime()} f=Linear",
				"SplineDoubleKeyFrame" => $"{Value()} @{KeyTime()} f=Spline",

				"LinearColorKeyFrame" => $"{Value()} @{KeyTime()} f=Linear",

				_ => throw new NotImplementedException($"{e.Name.LocalName} > {frame.Name.Pretty()}").PreDump(frame),
			};
		}
		Timeline ParseSimpleAnimation()
		{
			var result = ParseCommon();
			result.Value = e.Name.LocalName switch
			{
				"DoubleAnimation" => FormatAnimation(),
				"ColorAnimation" => FormatAnimation(),

				_ => throw new NotImplementedException(e.Name.Pretty()).PreDump(e),
			};

			return result;

			string FormatAnimation()
			{
				var from = SimplifyMarkup(e.Attribute("From")?.Value);
				var by = SimplifyMarkup(e.Attribute("By")?.Value);
				var to = SimplifyMarkup(e.Attribute("To")?.Value);
				var duration = SimplifyMarkup(SimplifyTimeSpan(e.Attribute("Duration")?.Value));

				return (from, by, to) switch
				{
					(_, null, null) => $"{from}->$base in {duration}",
					(null, _, null) => $"$current->{by} in {duration}",
					(null, null, _) => $"$current->{to} in {duration}",
					(_, null, _) => $"{from}->{to} in {duration}",
					(_, _, null) => $"{from}->{by} in {duration}",

					// -- https://learn.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.animation.doubleanimation?view=winrt-22621#remarks
					_ => throw new InvalidOperationException($"A DoubleAnimation typically has at least one of the From, By or To properties set, but never all three: from={from}, by={by}, to={to}"),
				};
			}
		}
		Timeline ParsePreconfiguredAnimation()
		{
			var result = ParseCommon();
			result.TargetName ??= e.Attribute("TargetName")?.Value;
			result.Value = result.Type;

			return result;
		}
	}
}
