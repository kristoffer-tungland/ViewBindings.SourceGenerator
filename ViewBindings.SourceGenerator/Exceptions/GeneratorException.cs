using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace ViewBindings.SourceGenerator.Exceptions;

public class GeneratorException : Exception
{
    public Location? Location { get; }

    public GeneratorException(ISymbol? symbol, string message) : base(message)
    {
        Location = symbol?.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax().GetLocation();
    }
}