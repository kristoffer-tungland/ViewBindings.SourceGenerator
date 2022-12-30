using System.Windows;
using ViewBindings.SourceGenerator.Demo.ViewModels;

namespace ViewBindings.SourceGenerator.Demo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Content = new MainViewModel();
    }
}