// !!! This control is not finished
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;

namespace Pelican.Avalonia; 

public partial class NavigationStackView : UserControl {
	public NavigationStackView() {
		InitializeComponent();
	}
	
	private PelicanRouter? _router;
	public static readonly DirectProperty<NavigationStackView, PelicanRouter?> RouterProperty =
		AvaloniaProperty.RegisterDirect<NavigationStackView, PelicanRouter?>(
			nameof(Router),
			o => o.Router,
			(o, v) => o.Router = v);
	public PelicanRouter? Router
	{
		get => _router;
		set {
			if (_router != null)
				_router.PropertyChanged -= PelicanRouterOnPropertyChanged;
			if (SetAndRaise(RouterProperty, ref _router, value))
				PelicanRouterChanged(value);
			if (value != null)
				value.PropertyChanged += PelicanRouterOnPropertyChanged;
		}
	}

	private void PelicanRouterChanged(PelicanRouter? value) {
	}

	private async void PelicanRouterOnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
		// var router = (PelicanRouter2)sender!;
		// if (e.PropertyName == nameof(PelicanRouter2.Route)) { // immutable route was changed
		// 	var pages = await Router!.BuildPages();
		// 	TransitionToPage(pages.LastOrDefault());
		// }
	}

	// public void TransitionToPage(IPelicanPage? page) {
	// 	Content = page;
	// }
	private void InputElement_OnTapped(object? sender, TappedEventArgs e) {
		Router?.Pop();
	}
	
	
	
	
	
	
	// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Toolbar %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
	
  /// <summary>
  /// Defines the <see cref="ToolbarContent"/> property.
  /// </summary>
  public static readonly StyledProperty<object?> ToolbarContentProperty =
      AvaloniaProperty.Register<NavigationStackView, object?>(nameof(ToolbarContent));

  /// <summary>
  /// Defines the <see cref="ToolbarContentTemplate"/> property.
  /// </summary>
  public static readonly StyledProperty<IDataTemplate?> ToolbarContentTemplateProperty =
      AvaloniaProperty.Register<NavigationStackView, IDataTemplate?>(nameof(ToolbarContentTemplate));

  // /// <summary>
  // /// Defines the <see cref="HorizontalContentAlignment"/> property.
  // /// </summary>
  // public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
  //     AvaloniaProperty.Register<ContentControl, HorizontalAlignment>(nameof(HorizontalContentAlignment));
  //
  // /// <summary>
  // /// Defines the <see cref="VerticalContentAlignment"/> property.
  // /// </summary>
  // public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
  //     AvaloniaProperty.Register<ContentControl, VerticalAlignment>(nameof(VerticalContentAlignment));

  /// <summary>
  /// Gets or sets the Toolbar content to display.
  /// </summary>
  [Content]
  [DependsOn(nameof(ToolbarContentTemplate))]
  public object? ToolbarContent
  {
      get => GetValue(ToolbarContentProperty);
      set => SetValue(ToolbarContentProperty, value);
  }

  /// <summary>
  /// Gets or sets the data template used to display the content of the Toolbar control.
  /// </summary>
  public IDataTemplate? ToolbarContentTemplate
  {
      get => GetValue(ToolbarContentTemplateProperty);
      set => SetValue(ToolbarContentTemplateProperty, value);
  }

  /// <summary>
  /// Gets the presenter from the Toolbar control's template.
  /// </summary>
  public ContentPresenter? ToolbarPresenter
  {
      get;
      private set;
  }

  // /// <summary>
  // /// Gets or sets the horizontal alignment of the content within the Toolbar control.
  // /// </summary>
  // public HorizontalAlignment HorizontalContentAlignment
  // {
  //     get => GetValue(HorizontalContentAlignmentProperty);
  //     set => SetValue(HorizontalContentAlignmentProperty, value);
  // }
  //
  // /// <summary>
  // /// Gets or sets the vertical alignment of the content within the Toolbar control.
  // /// </summary>
  // public VerticalAlignment VerticalContentAlignment
  // {
  //     get => GetValue(VerticalContentAlignmentProperty);
  //     set => SetValue(VerticalContentAlignmentProperty, value);
  // }

  // /// <inheritdoc/>
  // IAvaloniaList<ILogical> IContentPresenterHost.LogicalChildren => LogicalChildren;
  //
  // /// <inheritdoc/>
  // bool IContentPresenterHost.RegisterContentPresenter(ContentPresenter presenter)
  // {
  //     return RegisterContentPresenter(presenter);
  // }
  
  protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
  {
      base.OnPropertyChanged(change);

      if (change.Property == ToolbarContentProperty)
      {
	      ToolbarContentChanged(change);
      }
  }

  /// <summary>
  /// Called when an <see cref="ContentPresenter"/> is registered with the control.
  /// </summary>
  /// <param name="presenter">The presenter.</param>
  protected override bool RegisterContentPresenter(ContentPresenter presenter)
  {
      if (presenter.Name == "PART_ToolbarContentPresenter")
      {
	      ToolbarPresenter = presenter;
          return true;
      }
      return base.RegisterContentPresenter(presenter);
  }

  private void ToolbarContentChanged(AvaloniaPropertyChangedEventArgs e)
  {
      if (e.OldValue is ILogical oldChild)
      {
          LogicalChildren.Remove(oldChild);
      }

      if (e.NewValue is ILogical newChild)
      {
          LogicalChildren.Add(newChild);
      }
  }
}
