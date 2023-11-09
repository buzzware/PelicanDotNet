using AvaloniaCrossApp.Pelican;
using AvaloniaCrossApp.Utilities;

namespace AvaloniaCrossApp;

public class MainViewModel : ViewModelBase {
	public PelicanRouter2 Router => AppCommon.Router;
}
