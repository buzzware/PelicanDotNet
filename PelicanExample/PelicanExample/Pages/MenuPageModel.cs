using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PelicanExample.Utilities;

namespace PelicanExample.Pages; 

public class MenuPageModel : PageModel {
	
	public string Greeting => "Welcome to Avalonia!";

	public ICommand OneCommand { get; }

	public MenuPageModel() {
		OneCommand = new AsyncRelayCommand<string>(OnOne);
	}

	private async Task OnOne(string? arg) {
		
	}
}
