﻿//HintName: GeneratedViewBindings.g.cs
// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using System.Windows;

namespace ViewBindings.SourceGenerator.Demo.Resources;
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class GeneratedViewBindings : ResourceDictionary
{
    public GeneratedViewBindings()
    {
        AddDataTemplate(typeof(global::ViewBindings.SourceGenerator.Demo.ViewModels.SecondViewModel), typeof(global::ViewBindings.SourceGenerator.Demo.Views.SecondView));
    }

    void AddDataTemplate(Type viewModel, Type view)
    {
        var dataTemplate = new DataTemplate(viewModel)
        {VisualTree = new FrameworkElementFactory(view)};
        Add(new DataTemplateKey(viewModel), dataTemplate);
    }
}