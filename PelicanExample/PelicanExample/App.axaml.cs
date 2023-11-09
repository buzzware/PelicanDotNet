using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PelicanExample.Pages;
using PelicanExample.Views;

namespace PelicanExample;

public partial class App : Application
{
    public override void Initialize()
    {
        AppCommon.Setup();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var mainView = new MainView {
            DataContext = new MainViewModel()
        };
        //mainView.TransitionToPage(new MenuPage() {DataContext = new MenuPageModel()});
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            desktop.MainWindow = new MainWindow {
                DataContext = mainView.DataContext,
                Content = mainView
            };
        } else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {
            singleViewPlatform.MainView = mainView;
        }
        base.OnFrameworkInitializationCompleted();
    }
}
