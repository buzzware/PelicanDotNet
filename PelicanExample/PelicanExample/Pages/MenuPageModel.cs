using System.ComponentModel.Design;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Logging;
using AvaloniaCrossApp.Utilities;
using CommunityToolkit.Mvvm.Input;
using Serilog;

namespace AvaloniaCrossApp; 

public class MenuPageModel : PageModel {
	
	public string Greeting => "Welcome to Avalonia!";

	public ICommand OneCommand { get; }

	public MenuPageModel() {
		OneCommand = new AsyncRelayCommand<string>(OnOne);
	}

	private async Task OnOne(string? arg) {
		
	}
}
