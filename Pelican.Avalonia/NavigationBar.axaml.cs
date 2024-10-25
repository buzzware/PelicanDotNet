using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Pelican.Avalonia; 

public partial class NavigationBar : UserControl {
	
	protected override Type StyleKeyOverride => typeof(UserControl); // this is required when subclassing a component for the subclass to work like its parent
	
	private PelicanRouter? _router;
	public static readonly DirectProperty<NavigationBar, PelicanRouter?> RouterProperty =
		AvaloniaProperty.RegisterDirect<NavigationBar, PelicanRouter?>(
			nameof(Router),
			o => o.Router,
			(o, v) => o.Router = v);
	public PelicanRouter? Router
	{
		get => _router;
		set {
			// if (_router != null)
			// 	_router.PropertyChanged -= PelicanRouterOnPropertyChanged;
			// if (
			SetAndRaise(RouterProperty, ref _router, value);
			// )
			// 	PelicanRouterChanged(value);
			// if (value != null)
			// 	value.PropertyChanged += PelicanRouterOnPropertyChanged;
		}
	}
	
	public NavigationBar() {
		InitializeComponent();
	}
	
	private string? _titleText;
	public static readonly DirectProperty<NavigationBar, string?> TitleTextProperty =
		AvaloniaProperty.RegisterDirect<NavigationBar, string?>(
			nameof(TitleText),
			o => o.TitleText,
			(o, v) => o.TitleText = v);
	public string? TitleText {
		get => _titleText;
		set => SetAndRaise(TitleTextProperty, ref _titleText, value);
	}
	
	private int _backButtonSize = 40;
	public static readonly DirectProperty<NavigationBar, int> BackButtonSizeProperty =
		AvaloniaProperty.RegisterDirect<NavigationBar, int>(
			nameof(BackButtonSize),
			o => o.BackButtonSize,
			(o, v) => o.BackButtonSize = v);
	public int BackButtonSize {
		get => _backButtonSize;
		set => SetAndRaise(BackButtonSizeProperty, ref _backButtonSize, value);
	}
	
}
