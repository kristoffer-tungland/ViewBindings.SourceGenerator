using Microsoft.CodeAnalysis;
using ViewBindings.SourceGenerator.Extensions;

namespace ViewBindings.SourceGenerator.Exceptions;

public class ViewNotFoundException : GeneratorException
{
    public INamedTypeSymbol? ViewModelType { get; }

    public ViewNotFoundException(INamedTypeSymbol? viewModelType) : base(viewModelType, message: $"Suitable view not found for view model, expected view with name '{viewModelType.CalculateViewName()}'.")
    {
        ViewModelType = viewModelType;
    }
}