using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Hosting.Wpf.Core;
using Microsoft.Extensions.Hosting.Wpf.Locator;
using SimpleInjector;
using WpfIssue.Services;

namespace WpfIssue
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IViewModelLocatorInitialization<Container>, IApplicationInitializeComponent
    {
        public App()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        public void InitializeLocator(Container container)
        {
            // emulating scope
            using var scope = new Scope(container);
            var windowService = scope.GetInstance<WindowService>();
            windowService.ShowMainDialog();
        }

        public void Initialize()
        {
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ShowFatalException("An unhandled UI exception occurred.", e.Exception);
            e.Handled = true;
            Shutdown(-1);
        }

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                ShowFatalException("A fatal application exception occurred.", exception);
                return;
            }

            ShowFatalMessage("A fatal non-exception error occurred.");
        }

        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            ShowFatalException("An unobserved task exception occurred.", e.Exception);
            e.SetObserved();
        }

        private static void ShowFatalException(string message, Exception exception)
        {
            ShowFatalMessage($"{message}{Environment.NewLine}{Environment.NewLine}{exception}");
        }

        private static void ShowFatalMessage(string message)
        {
            MessageBox.Show(
                message,
                "Liisu Desktop Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
