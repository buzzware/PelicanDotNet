using Pelican;
using Pelican.Utilities;

namespace PelicanExample.Utilities;

public class ViewModelBase : BindableBase
{
	public PelicanRouter Router => AppCommon.Router;
}
