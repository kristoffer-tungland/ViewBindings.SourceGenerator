using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ViewBindings.SourceGenerator;

internal class GenerateViewBindingArgs
{
    public Compilation Compilation { get; }
    public IEnumerable<ClassDeclarationSyntax> ViewModels { get; }
    public List<ClassDeclarationSyntax> Views { get; }
    public string Namespace { get; }

    public GenerateViewBindingArgs(Compilation compilation, string? shortestNamespace, IEnumerable<ClassDeclarationSyntax> viewModels, List<ClassDeclarationSyntax> views)
    {
        Compilation = compilation;
        ViewModels = viewModels;
        Views = views;

        if (shortestNamespace is null)
            Namespace = "Resources";
        else
            Namespace = shortestNamespace + ".Resources";
    }

    public INamedTypeSymbol? GetDeclaredSymbol(ClassDeclarationSyntax classDeclarationSyntax, CancellationToken cancellationToken)
    {
        var semanticModel = Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
        return semanticModel.GetDeclaredSymbol(classDeclarationSyntax, cancellationToken);
    }
}