using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Hosting.Wpf.Core;
using Microsoft.Extensions.Hosting.Wpf.Locator;

namespace WpfIssue;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application, IViewModelLocatorInitialization<IServiceProvider>, IApplicationInitializeComponent
{
    public App()
    {
    }

    public void InitializeLocator(IServiceProvider serviceProvider)
    {
        var mainWindow = new MainWindow(serviceProvider);
        mainWindow.ShowDialog();
    }

    public void Initialize()
    {
    }
}