using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace WpfIssue.Services;

public class WindowService(Scope scope, IServiceScope serviceScope)
{
    public void ShowMainDialog()
    {
        // Demo purpose only, that somewhere a scoped service was initialized, we do it here for minimal code
        var someScope = scope.GetInstance<SomeScope>();
        someScope.Id = Guid.NewGuid();
        var mainWindow = new MainWindow(serviceScope.ServiceProvider, someScope);

        mainWindow.ShowDialog();
    }
}