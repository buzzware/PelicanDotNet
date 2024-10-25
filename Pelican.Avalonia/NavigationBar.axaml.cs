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
}
