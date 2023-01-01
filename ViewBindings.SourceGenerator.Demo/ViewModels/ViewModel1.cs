using ViewBindings.SourceGenerator.Contracts.Attributes;
using ViewBindings.SourceGenerator.Demo.Views;

namespace ViewBindings.SourceGenerator.Demo.ViewModels;

[ViewBinding(ViewType = typeof(View1))]
public class ViewModel1
{
    public string Text => "Hello from view model 1";
}