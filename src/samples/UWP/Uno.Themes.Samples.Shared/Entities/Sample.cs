using System;
using System.Collections.Generic;
using System.Text;
using Uno.Extensions;
using Uno.Logging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Uno.Themes.Samples.Entities
{
	[Bindable]
	public class Sample
	{
		public Sample(SamplePageAttribute attribute, Type viewType)
		{
			Category = attribute.Category;
			IconSource = attribute.IconSymbol == default ? (object)attribute.IconPath : (object)attribute.IconSymbol;
			Title = attribute.Title;
			Description = attribute.Description;
			DocumentationLink = attribute.DocumentationLink;
			Data = CreateData(attribute.DataType);
			Source = attribute.Source;
			SortOrder = attribute.SortOrder;

			ViewType = viewType;
		}

		private object CreateData(Type dataType)
		{
			if (dataType == null) return null;

			try
			{
				return Activator.CreateInstance(dataType);
			}
			catch (Exception e)
			{
				this.Log().Error($"Failed to initialize data for `{ViewType.Name}`:", e);
				return null;
			}
		}

		public SampleCategory Category { get; }

		public object IconSource { get; }

		public string Title { get; }

		public string Description { get; }

		public string DocumentationLink { get; }

		public object Data { get; }

		public int? SortOrder { get; }

		public SourceSdk Source { get; }

		public Type ViewType { get; }
	}
}
