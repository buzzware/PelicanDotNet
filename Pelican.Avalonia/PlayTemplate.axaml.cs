using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Pelican.Avalonia; 

public partial class PlayTemplate : UserControl {
	public PlayTemplate() {
		InitializeComponent();
	}
	
	// private PelicanRouter2? _router;
	// public static readonly DirectProperty<NavigationStackView, PelicanRouter2?> RouterProperty =
	// 	AvaloniaProperty.RegisterDirect<NavigationStackView, PelicanRouter2?>(
	// 		nameof(Router),
	// 		o => o.Router,
	// 		(o, v) => o.Router = v);
	// public PelicanRouter2? Router
	// {
	// 	get => _router;
	// 	set {
	// 		if (_router != null)
	// 			_router.PropertyChanged -= PelicanRouterOnPropertyChanged;
	// 		if (SetAndRaise(RouterProperty, ref _router, value))
	// 			PelicanRouterChanged(value);
	// 		if (value != null)
	// 			value.PropertyChanged += PelicanRouterOnPropertyChanged;
	// 	}
	// }
	//
	// private void PelicanRouterChanged(PelicanRouter2? value) {
	// }
	//
	// private async void PelicanRouterOnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
	// 	// var router = (PelicanRouter2)sender!;
	// 	// if (e.PropertyName == nameof(PelicanRouter2.Route)) { // immutable route was changed
	// 	// 	var pages = await Router!.BuildPages();
	// 	// 	TransitionToPage(pages.LastOrDefault());
	// 	// }
	// }
	//
	// public void TransitionToPage(IPelicanPage? page) {
	// 	Content = page;
	// }
}
