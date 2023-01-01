using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using ViewBindings.SourceGenerator.Attributes;

namespace ViewBindings.SourceGenerator;

[Generator]
public class ViewBindingsSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ClassAttributeReceiver(nameof(ViewBindingAttribute)));
    }

    public void Execute(GeneratorExecutionContext context)
    {
#if DEBUG
        //Debugger.Launch();
#endif
        if (context.SyntaxContextReceiver is not ClassAttributeReceiver receiver)
            return;

        // Get all types with the "ViewBinding" attribute
        var classesToRegister = receiver.Classes;
        var @namespace = GetNamespace(context);
        
        var source = GenerateCompositionRoot(@namespace+".Resources", classesToRegister);
        var sourceText = source.ToFullString();
        context.AddSource("GeneratedViewBindings.g.cs", SourceText.From(sourceText, Encoding.UTF8));
    }

    CompilationUnitSyntax GenerateCompositionRoot(string @namespace, List<INamedTypeSymbol> classesToRegister)
    {
        var compilationUnit = SyntaxFactory.CompilationUnit()
            .WithUsings(
            SyntaxFactory.List(
                new[]
                {
                    SyntaxFactory.UsingDirective(
                        SyntaxFactory.IdentifierName("System")),
                    SyntaxFactory.UsingDirective(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.IdentifierName("System"),
                            SyntaxFactory.IdentifierName("Windows")))
                }))
            .WithMembers(
            SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                SyntaxFactory.FileScopedNamespaceDeclaration(SyntaxFactory.IdentifierName(@namespace))
                .WithMembers(
                    SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                        SyntaxFactory.ClassDeclaration("GeneratedViewBindings")
                            .WithAttributeLists(
                                SyntaxFactory.SingletonList(
                                    SyntaxFactory.AttributeList(
                                        SyntaxFactory.SingletonSeparatedList(
                                            SyntaxFactory.Attribute(
                                                SyntaxFactory.QualifiedName(
                                                    SyntaxFactory.QualifiedName(
                                                        SyntaxFactory.QualifiedName(
                                                            SyntaxFactory.IdentifierName("System"),
                                                            SyntaxFactory.IdentifierName("Diagnostics")),
                                                        SyntaxFactory.IdentifierName("CodeAnalysis")),
                                                    SyntaxFactory.IdentifierName("ExcludeFromCodeCoverage")))))))
                            .WithModifiers(
                                SyntaxFactory.TokenList(
                                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                            .WithBaseList(
                                SyntaxFactory.BaseList(
                                    SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                                        SyntaxFactory.SimpleBaseType(
                                            SyntaxFactory.IdentifierName("ResourceDictionary")))))
                            .WithMembers(
                                SyntaxFactory.List(
                                    new MemberDeclarationSyntax[]
                                    {
                                        SyntaxFactory.ConstructorDeclaration(
                                                SyntaxFactory.Identifier("GeneratedViewBindings"))
                                            .WithModifiers(
                                                SyntaxFactory.TokenList(
                                                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                                            .WithBody(
                                                SyntaxFactory.Block(DataTemplatesToAdd(classesToRegister))),
                                        SyntaxFactory.MethodDeclaration(
                                            SyntaxFactory.PredefinedType(
                                                SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                                            SyntaxFactory.Identifier("AddDataTemplate"))
                                        .WithParameterList(
                                            SyntaxFactory.ParameterList(
                                                SyntaxFactory.SeparatedList<ParameterSyntax>(
                                                    new SyntaxNodeOrToken[]
                                                    {
                                                        SyntaxFactory.Parameter(
                                                                SyntaxFactory.Identifier("viewModel"))
                                                            .WithType(
                                                                SyntaxFactory.IdentifierName("Type")),
                                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                        SyntaxFactory.Parameter(
                                                                SyntaxFactory.Identifier("view"))
                                                            .WithType(
                                                                SyntaxFactory.IdentifierName("Type"))
                                                    })))
                                        .WithBody(
                                            SyntaxFactory.Block(
                                                SyntaxFactory.LocalDeclarationStatement(
                                                    SyntaxFactory.VariableDeclaration(
                                                            SyntaxFactory.IdentifierName(
                                                                SyntaxFactory.Identifier(
                                                                    SyntaxFactory.TriviaList(),
                                                                    SyntaxKind.VarKeyword,
                                                                    "var",
                                                                    "var",
                                                                    SyntaxFactory.TriviaList())))
                                        .WithVariables(
                                            SyntaxFactory.SingletonSeparatedList(
                                                SyntaxFactory.VariableDeclarator(
                                                        SyntaxFactory.Identifier("dataTemplate"))
                                                    .WithInitializer(
                                                        SyntaxFactory.EqualsValueClause(
                                                            SyntaxFactory.ObjectCreationExpression(
                                                                    SyntaxFactory.IdentifierName("DataTemplate"))
                                                        .WithArgumentList(
                                                            SyntaxFactory.ArgumentList(
                                                                SyntaxFactory.SingletonSeparatedList(
                                                                    SyntaxFactory.Argument(
                                                                        SyntaxFactory.IdentifierName("viewModel")))))
                                            .WithInitializer(
                                                SyntaxFactory.InitializerExpression(
                                                    SyntaxKind.ObjectInitializerExpression,
                                                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                    SyntaxFactory.AssignmentExpression(
                                                        SyntaxKind.SimpleAssignmentExpression,
                                                        SyntaxFactory.IdentifierName("VisualTree"),
                                                        SyntaxFactory.ObjectCreationExpression(
                                                                SyntaxFactory.IdentifierName("FrameworkElementFactory"))
                                                    .WithArgumentList(
                                                        SyntaxFactory.ArgumentList(
                                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                SyntaxFactory.Argument(
                                                                    SyntaxFactory.IdentifierName("view")))))))))))))),
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.IdentifierName("Add"))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                new SyntaxNodeOrToken[]
                                                {
                                                    SyntaxFactory.Argument(
                                                    SyntaxFactory.ObjectCreationExpression(
                                                            SyntaxFactory.IdentifierName("DataTemplateKey"))
                                                        .WithArgumentList(
                                                            SyntaxFactory.ArgumentList(
                                                                SyntaxFactory.SingletonSeparatedList(
                                                                    SyntaxFactory.Argument(
                                                                        SyntaxFactory.IdentifierName("viewModel")))))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("dataTemplate"))
                                                }))))))
                                    }))))))
            .NormalizeWhitespace();

        return compilationUnit;
    }

    static SyntaxList<StatementSyntax> DataTemplatesToAdd(List<INamedTypeSymbol> classesToRegister)
    {
        var statementSyntaxes = new List<StatementSyntax>();

        foreach (var type in classesToRegister.OrderBy(x => x.Name))
        {
            var attribute = type.GetAttributes().FirstOrDefault(x => x.AttributeClass?.Name == nameof(ViewBindingAttribute) || x.AttributeClass?.Name == nameof(ViewBindingAttribute).Replace("Attribute", ""));
            if (attribute is null)
                throw new ArgumentNullException(nameof(ViewBindingAttribute), "ViewBindingAttribute was not found on type");

            var namedArguments = attribute.NamedArguments;

            var viewTypeArgument = namedArguments.FirstOrDefault(arg => arg.Key == nameof(ViewBindingAttribute.ViewType));

            if (viewTypeArgument.Value.Value is not INamedTypeSymbol viewTypeSymbol)
                throw new ArgumentNullException(nameof(ViewBindingAttribute.ViewType), "View type was not set");

            var viewModelTypesAndNamespaces = GetNameAndContainingTypesAndNamespaces(type);
            var viewTypesAndNamespaces = GetNameAndContainingTypesAndNamespaces(viewTypeSymbol);

            var viewModelIdentifierNameSyntax = SyntaxFactory.ParseName(viewModelTypesAndNamespaces);
            var viewIdentifierNameSyntax = SyntaxFactory.IdentifierName(viewTypesAndNamespaces);

            var expressionStatement = SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName("AddDataTemplate"))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                    new SyntaxNodeOrToken[]
                    {
                        SyntaxFactory.Argument(
                            SyntaxFactory.TypeOfExpression(viewModelIdentifierNameSyntax)),
                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                        SyntaxFactory.Argument(
                            SyntaxFactory.TypeOfExpression(viewIdentifierNameSyntax))
                    }))));

            statementSyntaxes.Add(expressionStatement);
        }

        var result = new SyntaxList<StatementSyntax>(statementSyntaxes);

        return result;
    }

    static string GetNameAndContainingTypesAndNamespaces(ISymbol symbol)
    {
        return symbol.ToDisplayString(new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces));
    }

    static string GetNamespace(GeneratorExecutionContext context)
    {
        var @namespace = context.Compilation.SyntaxTrees
            .SelectMany(x => x.GetRoot().DescendantNodes())
            .OfType<NamespaceDeclarationSyntax>()
            .Select(x => x.Name.ToString())
            .Min();

        if (@namespace is not null)
            return @namespace;

        @namespace = context.Compilation.SyntaxTrees
            .SelectMany(x => x.GetRoot().DescendantNodes())
            .OfType<FileScopedNamespaceDeclarationSyntax>()
            .Select(x => x.Name.ToString())
            .Min();

        if (@namespace is not null)
            return @namespace;

        throw new NotSupportedException("Unable to calculate namespace");
    }
}