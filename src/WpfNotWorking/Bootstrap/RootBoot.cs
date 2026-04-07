using System.Reflection;
using Microsoft.Extensions.Hosting.Wpf.Bootstrap;
using SimpleInjector;
using WpfIssue.Services;

namespace WpfIssue.Bootstrap;

public sealed class RootBoot : IBootstrap<Container>
{
    public void Boot(Container container, Assembly[] assemblies)
    {
        container.Register<SomeScope>(Lifestyle.Scoped);
        container.Register<WindowService>(Lifestyle.Scoped);
    }

    public static Container CreateContainer()
    {
        return new Container
        {
            Options =
            {
                EnableAutoVerification = false,
                DefaultScopedLifestyle = ScopedLifestyle.Flowing
            }
        };
    }
}
