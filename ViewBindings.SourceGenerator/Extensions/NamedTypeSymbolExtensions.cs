using Microsoft.CodeAnalysis;

namespace ViewBindings.SourceGenerator.Extensions;

public static class NamedTypeSymbolExtensions
{
    public static string CalculateViewName(this INamedTypeSymbol namedTypeSymbol)
    {
        return namedTypeSymbol.Name.Replace("ViewModel", "View");
    }
}