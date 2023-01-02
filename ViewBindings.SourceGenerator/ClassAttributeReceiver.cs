using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ViewBindings.SourceGenerator.Extensions;


namespace ViewBindings.SourceGenerator;

public class ClassAttributeReceiver : ISyntaxContextReceiver
{
    public List<INamedTypeSymbol> ViewModels { get; } = new();
    public List<INamedTypeSymbol> AllViews { get; } = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax)
            return;

        INamedTypeSymbol? classSymbol = null;

        if (classDeclarationSyntax.Identifier.Text.EndsWith("View"))
        {
            classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) as INamedTypeSymbol;

            if (classSymbol is null)
                return;

            AllViews.Add(classSymbol);
        }

        if (!classDeclarationSyntax.HasViewBindingAttribute())
            return;

        classSymbol ??= context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) as INamedTypeSymbol;

        if (classSymbol is null)
            return;

        ViewModels.Add(classSymbol);
    }
}