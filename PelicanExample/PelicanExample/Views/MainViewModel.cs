using Pelican;
using PelicanExample.Utilities;

namespace PelicanExample.Views;

public class MainViewModel : ViewModelBase {
	public PelicanRouter2 Router => AppCommon.Router;
}
