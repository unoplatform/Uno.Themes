using System.Threading.Tasks;

#if WinUI
using Microsoft.UI.Xaml.Media.Animation;
#else
using Windows.UI.Xaml.Media.Animation;
#endif

namespace Uno.Themes;

internal static class StoryboardExtensions
{
	/// <summary>
	/// Begins a Storyboard and await for its completion
	/// </summary>
	/// <param name="storyboard">The storyboard</param>
	internal static async Task Run(this Storyboard storyboard)
	{
		var cts = new TaskCompletionSource<bool>();
		void OnCompleted(object sender, object e)
		{
			cts.SetResult(true);
			storyboard.Completed -= OnCompleted;
		}

		storyboard.Completed += OnCompleted;
		storyboard.Begin();
		await cts.Task;
	}
}
