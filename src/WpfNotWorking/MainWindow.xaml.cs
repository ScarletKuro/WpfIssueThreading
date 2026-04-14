using System.ComponentModel;
using System.Windows;

namespace WpfIssue;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IAsyncDisposable
{
    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BlazorWebView.Services = serviceProvider;
    }

    public async ValueTask DisposeAsync()
    {
        if (BlazorWebView is not null)
        {
            await BlazorWebView.DisposeAsync();
        }
    }
}