using System;
using System.Windows;

namespace ViewBindings.SourceGenerator.Demo.Resources
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GeneratedViewBindings : ResourceDictionary
    {
        public GeneratedViewBindings()
        {
            AddDataTemplate(typeof(ViewBindings.SourceGenerator.Demo.ViewModels.MainViewModel), typeof(ViewBindings.SourceGenerator.Demo.Views.MainView));
        }

        void AddDataTemplate(Type viewModel, Type view)
        {
            var dataTemplate = new DataTemplate(viewModel) { VisualTree = new FrameworkElementFactory(view) };
            Add(new DataTemplateKey(viewModel), dataTemplate);
        }
    }
}
