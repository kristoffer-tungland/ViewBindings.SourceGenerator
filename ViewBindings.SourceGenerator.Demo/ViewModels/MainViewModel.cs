﻿using System.Collections.Generic;
using ViewBindings.SourceGenerator.Contracts.Attributes;
using ViewBindings.SourceGenerator.Demo.Views;

namespace ViewBindings.SourceGenerator.Demo.ViewModels;

[ViewBinding(typeof(MainView))]
public class MainViewModel
{
    public List<object> Views { get; } = new();

    public MainViewModel()
    {
        //Views.Add();
    }
}