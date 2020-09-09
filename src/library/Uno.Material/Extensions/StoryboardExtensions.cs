using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Uno.Material.Extensions
{
	/// <summary>
	/// Extension methods for classes in the Windows.UI.Xaml.Media.Animation namespace.
	/// </summary>
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
}
