using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ViewBindings.SourceGenerator;

public class ClassAttributeReceiver : ISyntaxContextReceiver
{
    private readonly string _expectedAttribute;
    public ClassAttributeReceiver(string expectedAttribute) => _expectedAttribute = expectedAttribute;

    public List<INamedTypeSymbol> Classes { get; } = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax)
            return;

        if (!HasAttribute(classDeclarationSyntax))
            return;

        if (context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
            return;

        Classes.Add(classSymbol);
    }
    protected bool HasAttribute(ClassDeclarationSyntax classDeclarationSyntax)
    {
        foreach (var attributeList in classDeclarationSyntax.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                if (attribute.Name.ToString() == _expectedAttribute || attribute.Name.ToString() + "Attribute" == _expectedAttribute)
                    return true;
            }
        }
        return false;
    }
}