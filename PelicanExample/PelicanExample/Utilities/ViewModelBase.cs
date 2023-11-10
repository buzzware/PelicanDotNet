using Pelican;
using Pelican.Utilities;

namespace PelicanExample.Utilities;

public class ViewModelBase : BindableBase
{
	public PelicanRouter2 Router => AppCommon.Router;
}
