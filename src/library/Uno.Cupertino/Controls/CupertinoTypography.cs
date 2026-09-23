#nullable enable

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace Uno.Cupertino;

// Uno's ContentPresenter.CharacterSpacing does not currently propagate to its generated text.
// Opt in only our default string presenters; consumer templates and UIElement content stay untouched.
internal static class CupertinoTypography
{
	public static readonly DependencyProperty BindCharacterSpacingProperty = DependencyProperty.RegisterAttached(
		"BindCharacterSpacing", typeof(bool), typeof(CupertinoTypography), new PropertyMetadata(false, OnEnabledChanged));

	private static readonly DependencyProperty StateProperty = DependencyProperty.RegisterAttached(
		"State", typeof(PresenterState), typeof(CupertinoTypography), new PropertyMetadata(null));

	public static bool GetBindCharacterSpacing(DependencyObject element) => (bool)element.GetValue(BindCharacterSpacingProperty);
	public static void SetBindCharacterSpacing(DependencyObject element, bool value) => element.SetValue(BindCharacterSpacingProperty, value);

	private static void OnEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is not ContentPresenter presenter)
		{
			return;
		}
		if (presenter.GetValue(StateProperty) is PresenterState previous)
		{
			previous.Detach();
			presenter.ClearValue(StateProperty);
		}
		if (args.NewValue is true)
		{
			var state = new PresenterState(presenter);
			presenter.SetValue(StateProperty, state);
			state.Attach();
		}
	}

	private sealed class PresenterState(ContentPresenter presenter)
	{
		private long _contentToken;
		private long _templateToken;
		private long _selectorToken;
		private bool _listening;
		private bool _layoutPending;
		private TextBlock? _text;

		public void Attach()
		{
			presenter.Loaded += OnLoaded;
			presenter.Unloaded += OnUnloaded;
			if (presenter.IsLoaded)
			{
				Start();
			}
		}

		public void Detach()
		{
			Stop();
			presenter.Loaded -= OnLoaded;
			presenter.Unloaded -= OnUnloaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs args) => Start();
		private void OnUnloaded(object sender, RoutedEventArgs args) => Stop();

		private void Start()
		{
			if (!_listening)
			{
				_contentToken = presenter.RegisterPropertyChangedCallback(ContentPresenter.ContentProperty, OnContentChanged);
				_templateToken = presenter.RegisterPropertyChangedCallback(ContentPresenter.ContentTemplateProperty, OnContentChanged);
				_selectorToken = presenter.RegisterPropertyChangedCallback(ContentPresenter.ContentTemplateSelectorProperty, OnContentChanged);
				_listening = true;
			}
			Apply();
			Schedule();
		}

		private void Stop()
		{
			if (_listening)
			{
				presenter.UnregisterPropertyChangedCallback(ContentPresenter.ContentProperty, _contentToken);
				presenter.UnregisterPropertyChangedCallback(ContentPresenter.ContentTemplateProperty, _templateToken);
				presenter.UnregisterPropertyChangedCallback(ContentPresenter.ContentTemplateSelectorProperty, _selectorToken);
				_listening = false;
			}
			CancelLayout();
			ClearText();
		}

		private void OnContentChanged(DependencyObject sender, DependencyProperty property)
		{
			ClearText();
			Schedule();
		}

		private void Schedule()
		{
			if (!_layoutPending)
			{
				presenter.LayoutUpdated += OnLayoutUpdated;
				_layoutPending = true;
			}
		}

		private void CancelLayout()
		{
			if (_layoutPending)
			{
				presenter.LayoutUpdated -= OnLayoutUpdated;
				_layoutPending = false;
			}
		}

		private void OnLayoutUpdated(object? sender, object args)
		{
			CancelLayout();
			Apply();
		}

		private void Apply()
		{
			if (presenter.ContentTemplate is not null || presenter.ContentTemplateSelector is not null
				|| presenter.Content is null or UIElement || VisualTreeHelper.GetChildrenCount(presenter) == 0)
			{
				return;
			}
			if (VisualTreeHelper.GetChild(presenter, 0) is TextBlock text && !ReferenceEquals(_text, text))
			{
				ClearText();
				_text = text;
				text.SetBinding(TextBlock.CharacterSpacingProperty, new Binding
				{
					Source = presenter,
					Path = new PropertyPath(nameof(ContentPresenter.CharacterSpacing)),
				});
			}
		}

		private void ClearText()
		{
			_text?.ClearValue(TextBlock.CharacterSpacingProperty);
			_text = null;
		}
	}
}
