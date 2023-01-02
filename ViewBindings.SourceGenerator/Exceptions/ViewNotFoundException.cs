using System;
using Microsoft.CodeAnalysis;

namespace ViewBindings.SourceGenerator.Exceptions;

public class ViewNotFoundException : Exception
{
    public INamedTypeSymbol ViewModelType { get; }

    public ViewNotFoundException(INamedTypeSymbol viewModelType)
    {
        ViewModelType = viewModelType;
    }
}