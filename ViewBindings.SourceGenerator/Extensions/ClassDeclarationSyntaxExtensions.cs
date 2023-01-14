using Microsoft.CodeAnalysis.CSharp.Syntax;
using ViewBindings.SourceGenerator.Contracts.Attributes;

namespace ViewBindings.SourceGenerator.Extensions;

public static class ClassDeclarationSyntaxExtensions
{
    public static bool HasViewBindingAttribute(this ClassDeclarationSyntax classDeclarationSyntax)
    {
        foreach (var attributeList in classDeclarationSyntax.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                if (attribute.Name.ToString() == nameof(ViewBindingAttribute) || attribute.Name + "Attribute" == nameof(ViewBindingAttribute))
                    return true;
            }
        }
        return false;
    }
}