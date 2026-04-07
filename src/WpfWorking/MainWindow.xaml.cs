using System.Windows;
using System.Windows.Media.Animation;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Web.WebView2.Core;

namespace WpfIssue;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{


    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BlazorWebView.Services = serviceProvider;
    }

}