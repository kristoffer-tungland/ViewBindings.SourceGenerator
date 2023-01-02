using ViewBindings.SourceGenerator.Contracts.Attributes;
using ViewBindings.SourceGenerator.Demo.Views;

namespace ViewBindings.SourceGenerator.Demo.ViewModels;

[ViewBinding(ViewType = typeof(FirstView))]
public class FirstViewModel
{
    public string Text => "Hello from view model 1";
}