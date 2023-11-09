using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Pelican;
using PelicanExample.Utilities;

namespace PelicanExample.Pages; 

public class MenuPageModel : PageModel {
	
	public ICommand PushCommand { get; }

	public MenuPageModel() {
		PushCommand = new AsyncRelayCommand<string>(OnPush);
	}

	private async Task OnPush(string? arg) {
		if (arg != null)
			await AppCommon.Router.Push(new PelicanRouteSegment(arg));
	}
}
