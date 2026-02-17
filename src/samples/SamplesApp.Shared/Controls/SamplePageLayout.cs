namespace Uno.Themes.Samples;

internal sealed class ActionDisposable : IDisposable
{
	private Action _action;
	public ActionDisposable(Action action) => _action = action;
	public void Dispose() => System.Threading.Interlocked.Exchange(ref _action, null)?.Invoke();
}

public partial class SamplePageLayout : ContentControl
{
	private const string VisualStateMaterial = nameof(Design.Material);
	private const string VisualStateCupertino = nameof(Design.Cupertino);

	private const string MaterialRadioButtonPartName = "PART_MaterialRadioButton";
	private const string CupertinoRadioButtonPartName = "PART_CupertinoRadioButton";
	private const string StickyMaterialRadioButtonPartName = "PART_StickyMaterialRadioButton";
	private const string StickyCupertinoRadioButtonPartName = "PART_StickyCupertinoRadioButton";
	private const string ScrollingTabsPartName = "PART_ScrollingTabs";
	private const string StickyTabsPartName = "PART_StickyTabs";
	private const string ScrollViewerPartName = "PART_ScrollViewer";
	private const string TopPartName = "PART_MobileTopBar";
	private const string MaterialVersionComboBoxName = "MaterialVersionComboBox";

	private static Design _design = Design.Material;

	/// <summary>
	/// Gets or sets the active design for the sample app. Set this once at app startup.
	/// </summary>
	public static Design ActiveDesign
	{
		get => _design;
		set => _design = value;
	}

	private IReadOnlyCollection<LayoutModeMapping> LayoutModeMappings => new List<LayoutModeMapping>
	{
		new LayoutModeMapping(Design.Material, _materialRadioButton, _stickyMaterialRadioButton, VisualStateMaterial, /* templates: */ MaterialTemplate, M3MaterialTemplate),
		new LayoutModeMapping(Design.Cupertino, _cupertinoRadioButton, _stickyCupertinoRadioButton, VisualStateCupertino, /* templates: */ CupertinoTemplate),
	};

	private RadioButton _materialRadioButton;
	private RadioButton _cupertinoRadioButton;
	private RadioButton _stickyMaterialRadioButton;
	private RadioButton _stickyCupertinoRadioButton;
	private FrameworkElement _scrollingTabs;
	private FrameworkElement _stickyTabs;
	private FrameworkElement _top;
	private ScrollViewer _scrollViewer;
	private ComboBox _materialVersionComboBox;

	private IDisposable _subscriptions;

	public SamplePageLayout()
	{
		DataContextChanged += OnDataContextChanged;

		void OnDataContextChanged(object sender, DataContextChangedEventArgs args)
		{
			if (args.NewValue is Sample sample)
			{
				Title = sample.Title;
				Description = sample.Description;
				DocumentationLink = sample.DocumentationLink;
				Source = sample.Source;
			}
		}
	}

	protected override void OnApplyTemplate()
	{
		base.OnApplyTemplate();

		_materialRadioButton = (RadioButton)GetTemplateChild(MaterialRadioButtonPartName);
		_cupertinoRadioButton = (RadioButton)GetTemplateChild(CupertinoRadioButtonPartName);
		_stickyMaterialRadioButton = (RadioButton)GetTemplateChild(StickyMaterialRadioButtonPartName);
		_stickyCupertinoRadioButton = (RadioButton)GetTemplateChild(StickyCupertinoRadioButtonPartName);
		_scrollingTabs = (FrameworkElement)GetTemplateChild(ScrollingTabsPartName);
		_stickyTabs = (FrameworkElement)GetTemplateChild(StickyTabsPartName);
		_scrollViewer = (ScrollViewer)GetTemplateChild(ScrollViewerPartName);
		_top = (FrameworkElement)GetTemplateChild(TopPartName);
		_materialVersionComboBox = (ComboBox)GetTemplateChild(MaterialVersionComboBoxName);

		// ensure previous subscriptions is removed before adding new ones, in case OnApplyTemplate is called multiple times
		_subscriptions?.Dispose();

		_scrollViewer.ViewChanged += OnScrolled;

		BindOnClick(_materialRadioButton);
		BindOnClick(_cupertinoRadioButton);
		BindOnClick(_stickyMaterialRadioButton);
		BindOnClick(_stickyCupertinoRadioButton);

		UpdateLayoutRadioButtons();

		void BindOnClick(RadioButton radio)
		{
			radio.Click += OnLayoutRadioButtonChecked;
		}

		_materialVersionComboBox.Loaded += UpdateDefaultMaterialVersionChoice;

		_subscriptions = new ActionDisposable(() =>
		{
			_scrollViewer.ViewChanged -= OnScrolled;
			_materialRadioButton.Click -= OnLayoutRadioButtonChecked;
			_cupertinoRadioButton.Click -= OnLayoutRadioButtonChecked;
			_stickyMaterialRadioButton.Click -= OnLayoutRadioButtonChecked;
			_stickyCupertinoRadioButton.Click -= OnLayoutRadioButtonChecked;
			_materialVersionComboBox.Loaded -= UpdateDefaultMaterialVersionChoice;
		});

		void OnScrolled(object sender, ScrollViewerViewChangedEventArgs e)
		{
			var relativeOffset = GetRelativeOffset();
			if (relativeOffset < 0)
			{
				_stickyTabs.Visibility = Visibility.Visible;
			}
			else
			{
				_stickyTabs.Visibility = Visibility.Collapsed;
			}
		}
	}

	private void UpdateDefaultMaterialVersionChoice(object sender, RoutedEventArgs e)
	{
		if (LayoutModeMappings.FirstOrDefault(x => x.Design == Design.Material) is LayoutModeMapping materialMapping)
		{
			_materialVersionComboBox.SelectedIndex = materialMapping.Templates
				.Select((x, i) => new { Index = i, Template = x })
				.LastOrDefault(x => x.Template != null) // only material design has multiple templates, v1+m3, and the latest is favored for default choice.
				?.Index ?? -1;
		}
	}

	private void UpdateLayoutRadioButtons()
	{
		var mappings = LayoutModeMappings;

		// In single-theme mode: hide all design-switching tabs
		foreach (var mapping in mappings)
		{
			mapping.RadioButton.Visibility = Visibility.Collapsed;
			mapping.StickyRadioButton.Visibility = Visibility.Collapsed;
		}

		// Hide the scrolling and sticky tab bars entirely
		if (_scrollingTabs != null) _scrollingTabs.Visibility = Visibility.Collapsed;
		if (_stickyTabs != null) _stickyTabs.Visibility = Visibility.Collapsed;

		// Hide the Material version combo box and force M3 selection
		if (_materialVersionComboBox != null)
		{
			_materialVersionComboBox.Visibility = Visibility.Collapsed;
			// Select M3 (index 1) to ensure M3 content is shown via bindings
			_materialVersionComboBox.SelectedIndex = 1;
		}

		// Activate the design matching the app's active theme
		var selected = mappings.FirstOrDefault(x => x.Design == ActiveDesign && x.HasDefinedTemplate)
			?? mappings.FirstOrDefault(x => x.HasDefinedTemplate);
		if (selected != null)
		{
			UpdateLayoutMode(transitionTo: selected.Design);
		}
	}

	private void OnLayoutRadioButtonChecked(object sender, RoutedEventArgs e)
	{
		if (sender is RadioButton radio && LayoutModeMappings.FirstOrDefault(x => x.RadioButton == radio || x.StickyRadioButton == radio) is LayoutModeMapping mapping)
		{
			_design = mapping.Design;
			UpdateLayoutMode();
		}
	}

	private void UpdateLayoutMode(Design? transitionTo = null)
	{
		var design = transitionTo ?? _design;

		var current = LayoutModeMappings.FirstOrDefault(x => x.Design == design);
		if (current != null)
		{
			current.RadioButton.IsChecked = true;
			current.StickyRadioButton.IsChecked = true;

			VisualStateManager.GoToState(this, current.VisualStateName, useTransitions: true);
		}
	}

	private double GetRelativeOffset()
	{
#if NETFX_CORE
		// On UWP we can count on finding a ScrollContentPresenter. 
		var scp = VisualTreeHelperEx.FindFirstDescendant<ScrollContentPresenter>(_scrollViewer);
		var content = scp?.Content as FrameworkElement;
		var transform = _scrollingTabs.TransformToVisual(content);
		return transform.TransformPoint(new Point(0, 0)).Y - _scrollViewer.VerticalOffset;
#elif __IOS__
		var transform = _scrollingTabs.TransformToVisual(_scrollViewer);
		return transform.TransformPoint(new Point(0, 0)).Y;
#else
		var transform = _scrollingTabs.TransformToVisual(this);
		return transform.TransformPoint(new Point(0, 0)).Y - _top.ActualHeight;
#endif
	}

	/// <summary>
	/// Get control inside the specified layout template.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="mode">The layout mode in which the control is defined</param>
	/// <param name="name">The 'x:Name' of the control</param>
	/// <returns></returns>
	/// <remarks>The caller must ensure the control is loaded. This is best done from <see cref="FrameworkElement.Loaded"/> event.</remarks>
	public T GetSampleChild<T>(Design mode, string name)
		where T : FrameworkElement
	{
		var presenter = (ContentPresenter)GetTemplateChild($"{mode}ContentPresenter");

		return VisualTreeHelperEx.FindFirstDescendant<T>(presenter, x => x.Name == name);
	}

	private class LayoutModeMapping
	{
		public Design Design { get; }
		public RadioButton RadioButton { get; }
		public RadioButton StickyRadioButton { get; }
		public string VisualStateName { get; }
		public DataTemplate[] Templates { get; }
		public bool HasDefinedTemplate => Templates.Any(x => x != null);

		public LayoutModeMapping(Design design, RadioButton radioButton, RadioButton stickyRadioButton, string visualStateName, params DataTemplate[] templates)
		{
			Design = design;
			RadioButton = radioButton;
			StickyRadioButton = stickyRadioButton;
			VisualStateName = visualStateName;
			Templates = templates;
		}
	}
}
