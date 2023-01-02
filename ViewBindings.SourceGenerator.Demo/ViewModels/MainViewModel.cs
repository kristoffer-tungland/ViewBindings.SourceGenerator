using System.Collections.Generic;
using ViewBindings.SourceGenerator.Contracts.Attributes;

namespace ViewBindings.SourceGenerator.Demo.ViewModels;

[ViewBinding]
public class MainViewModel
{
    public List<object> Views { get; } = new();

    public MainViewModel()
    {
        Views.Add(new FirstViewModel());
        Views.Add(new SecondViewModel());
    }
}