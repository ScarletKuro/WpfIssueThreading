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

    protected override void OnStartup(StartupEventArgs e)
    {
        SetupExceptionHandling();
        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        UnSetupExceptionHandling();
    }

    private void UnSetupExceptionHandling()
    {
        AppDomain.CurrentDomain.UnhandledException -= OnCurrentDomainUnhandledException;
        DispatcherUnhandledException -= OnDispatcherUnhandledException;
        TaskScheduler.UnobservedTaskException -= OnUnobservedTaskException;
    }

    private void SetupExceptionHandling()
    {
        AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        ShowFatalException("An unhandled UI exception occurred.", e.Exception);
        e.Handled = true;
    }

    private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ShowFatalMessage("A fatal non-exception error occurred.");
    }

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        ShowFatalException("An unobserved task exception occurred.", e.Exception);
        e.SetObserved();
    }

    private void ShowFatalException(string message, Exception exception)
    {
        ShowFatalMessage($"{message}{Environment.NewLine}{Environment.NewLine}{exception}");
    }

    private static void ShowFatalMessage(string message)
    {
        MessageBox.Show(
            message,
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}