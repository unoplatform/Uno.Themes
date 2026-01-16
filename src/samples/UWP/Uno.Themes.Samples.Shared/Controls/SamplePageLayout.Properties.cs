namespace Uno.Themes.Samples;

public partial class SamplePageLayout
{
	#region Property: Title

	public static DependencyProperty TitleProperty { get; } = DependencyProperty.Register(
		nameof(Title),
		typeof(string),
		typeof(SamplePageLayout),
		new PropertyMetadata(default));

	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	#endregion
	#region Property: Description

	public static DependencyProperty DescriptionProperty { get; } = DependencyProperty.Register(
		nameof(Description),
		typeof(string),
		typeof(SamplePageLayout),
		new PropertyMetadata(default));

	public string Description
	{
		get => (string)GetValue(DescriptionProperty);
		set => SetValue(DescriptionProperty, value);
	}

	#endregion
	#region Property: DocumentationLink

	public static DependencyProperty DocumentationLinkProperty { get; } = DependencyProperty.Register(
		nameof(DocumentationLink),
		typeof(string),
		typeof(SamplePageLayout),
		new PropertyMetadata(default));

	public string DocumentationLink
	{
		get => (string)GetValue(DocumentationLinkProperty);
		set => SetValue(DocumentationLinkProperty, value);
	}

	#endregion
	#region Property: Source

	public static DependencyProperty SourceProperty { get; } = DependencyProperty.Register(
		nameof(Source),
		typeof(SourceSdk?),
		typeof(SamplePageLayout),
		new PropertyMetadata(default));

	public SourceSdk? Source
	{
		get => (SourceSdk?)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	#endregion

	#region Property: CupertinoTemplate

	public static DependencyProperty CupertinoTemplateProperty { get; } = DependencyProperty.Register(
		nameof(CupertinoTemplate),
		typeof(DataTemplate),
		typeof(SamplePageLayout),
		new PropertyMetadata(default));

	public DataTemplate CupertinoTemplate
	{
		get => (DataTemplate)GetValue(CupertinoTemplateProperty);
		set => SetValue(CupertinoTemplateProperty, value);
	}

	#endregion
	#region Property: MaterialTemplate

	public static DependencyProperty MaterialTemplateProperty { get; } = DependencyProperty.Register(
		nameof(MaterialTemplate),
		typeof(DataTemplate),
		typeof(SamplePageLayout),
		new PropertyMetadata(default));

	public DataTemplate MaterialTemplate
	{
		get => (DataTemplate)GetValue(MaterialTemplateProperty);
		set => SetValue(MaterialTemplateProperty, value);
	}

	#endregion

	#region Property: M3MaterialTemplate

	public static DependencyProperty M3MaterialTemplateProperty { get; } = DependencyProperty.Register(
		nameof(M3MaterialTemplate),
		typeof(DataTemplate),
		typeof(SamplePageLayout),
		new PropertyMetadata(default));

	public DataTemplate M3MaterialTemplate
	{
		get => (DataTemplate)GetValue(M3MaterialTemplateProperty);
		set => SetValue(M3MaterialTemplateProperty, value);
	}

	#endregion

	#region Property: SimpleTemplate

	public static DependencyProperty SimpleTemplateProperty { get; } = DependencyProperty.Register(
		nameof(SimpleTemplate),
		typeof(DataTemplate),
		typeof(SamplePageLayout),
		new PropertyMetadata(default));

	public DataTemplate SimpleTemplate
	{
		get => (DataTemplate)GetValue(SimpleTemplateProperty);
		set => SetValue(SimpleTemplateProperty, value);
	}

	#endregion

	#region Property: HeaderTemplate
	/// <summary>
	/// The Header is the part above the design tabs (Material|Fluent|Native).
	/// It contains the Description and the Source in the default style.
	/// </summary>
	public DataTemplate HeaderTemplate
	{
		get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
		set { SetValue(HeaderTemplateProperty, value); }
	}

	public static readonly DependencyProperty HeaderTemplateProperty =
		DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(SamplePageLayout), new PropertyMetadata(null));
	#endregion
}
