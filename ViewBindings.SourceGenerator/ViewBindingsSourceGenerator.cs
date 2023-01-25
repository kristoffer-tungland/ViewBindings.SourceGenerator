using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using ViewBindings.SourceGenerator.Contracts.Attributes;
using ViewBindings.SourceGenerator.Exceptions;
using ViewBindings.SourceGenerator.Extensions;

namespace ViewBindings.SourceGenerator;

[Generator]
public class ViewBindingsSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        //Debugger.Launch();
#endif
        // Do a simple filter for viewModels
        IncrementalValuesProvider<ClassDeclarationSyntax> viewModelDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s), // select enums with attributes
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx)) // sect the enum with the [EnumExtensions] attribute
            .Where(static m => m is not null)!; // filter out attributed enums that we don't care about

        // Combine the selected enums with the `Compilation`
        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndViewModels
            = context.CompilationProvider.Combine(viewModelDeclarations.Collect());

        // Generate the source using the compilation and enums
        context.RegisterSourceOutput(compilationAndViewModels,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

    static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        // we know the node is a ClassDeclarationSyntax thanks to IsSyntaxTargetForGeneration
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        // loop through all the attributes on the method
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                {
                    // weird, we couldn't get the symbol, ignore it
                    continue;
                }

                var attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                var fullName = attributeContainingTypeSymbol.ToDisplayString();

                // Is the attribute the [ViewBindingAttribute] attribute?
                if (fullName == "ViewBindings.SourceGenerator.Contracts.Attributes.ViewBindingAttribute")
                {
                    // return the enum
                    return classDeclarationSyntax;
                }
            }
        }

        // we didn't find the attribute we were looking for
        return null;
    }

    private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> viewModels, SourceProductionContext context)
    {
        if (viewModels.IsDefaultOrEmpty)
        {
            // nothing to do yet
            return;
        }

        // I'm not sure if this is actually necessary, but `[LoggerMessage]` does it, so seems like a good idea!
        var distinctViewModels = viewModels.Distinct();

        try
        {
            var generateViewBindingArgs = CreateViewBindingsArgs(compilation, distinctViewModels);

            var source = GenerateViewBindingResources(generateViewBindingArgs, context.CancellationToken);
            var sourceText = source.ToFullString();
            context.AddSource("GeneratedViewBindings.g.cs", SourceText.From(sourceText, Encoding.UTF8));
        }
        catch (ViewNotFoundException viewNotFoundException)
        {
            var descriptor = new DiagnosticDescriptor("CS0103", "Suitable view not found for view model", viewNotFoundException.Message, "ViewBindings", DiagnosticSeverity.Error, true);
            context.ReportDiagnostic(Diagnostic.Create(descriptor, viewNotFoundException.Location));
        }
        catch (GeneratorException generatorException)
        {
            var descriptor = new DiagnosticDescriptor("CS8603", "Generator exception", generatorException.Message, "Exception", DiagnosticSeverity.Error, true);
            context.ReportDiagnostic(Diagnostic.Create(descriptor, generatorException.Location));

        }
    }

    static CompilationUnitSyntax GenerateViewBindingResources(GenerateViewBindingArgs args, CancellationToken cancellationToken)
    {
        var @namespace = args.Namespace;

        var compilationUnit = SyntaxFactory.CompilationUnit()
            .WithUsings(
            SyntaxFactory.List(
                new[]
                {
                    SyntaxFactory.UsingDirective(
                            SyntaxFactory.IdentifierName("System"))
                        .WithUsingKeyword(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(
                                    new []{
                                        SyntaxFactory.Comment("// <auto-generated/>"),
                                        SyntaxFactory.Trivia(
                                            SyntaxFactory.PragmaWarningDirectiveTrivia(
                                                SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                                                true)),
                                        SyntaxFactory.Trivia(
                                            SyntaxFactory.NullableDirectiveTrivia(
                                                SyntaxFactory.Token(SyntaxKind.EnableKeyword),
                                                true))}),
                                SyntaxKind.UsingKeyword,
                                SyntaxFactory.TriviaList())),
                    SyntaxFactory.UsingDirective(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.IdentifierName("System"),
                            SyntaxFactory.IdentifierName("Windows")))}))
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
                                                            SyntaxFactory.AliasQualifiedName(
                                                                SyntaxFactory.IdentifierName(
                                                                    SyntaxFactory.Token(SyntaxKind.GlobalKeyword)),
                                                                SyntaxFactory.IdentifierName("System")),
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
                                                SyntaxFactory.Block(DataTemplatesToAdd(args, cancellationToken))),
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


    static SyntaxList<StatementSyntax> DataTemplatesToAdd(GenerateViewBindingArgs args, CancellationToken cancellationToken)
    {
        var statementSyntaxes = new List<StatementSyntax>();

        foreach (var viewModelDeclarationSyntax in args.ViewModels.OrderBy(x => x.Identifier.Text))
        {
            var viewModelType = args.GetDeclaredSymbol(viewModelDeclarationSyntax, cancellationToken);

            if (viewModelType is null)
                throw new GeneratorException(viewModelType, $"Could not get declared symbol from {viewModelDeclarationSyntax.Identifier.ToFullString()}");

            var attribute = viewModelType.GetAttributes().FirstOrDefault(x => x.AttributeClass?.Name == nameof(ViewBindingAttribute) || x.AttributeClass?.Name == nameof(ViewBindingAttribute).Replace("Attribute", ""));

            if (attribute is null)
                throw new GeneratorException(viewModelType, $"{nameof(ViewBindingAttribute)} was not found on view model");

            INamedTypeSymbol? viewTypeSymbol = null;
            var namedArguments = attribute.NamedArguments;

            // Check if the view is specified on argument
            if (!namedArguments.IsEmpty && namedArguments.FirstOrDefault(arg =>
                    arg.Key == nameof(ViewBindingAttribute.ViewType)) is { } viewTypeArgument)
            {
                if (viewTypeArgument.Value.Kind != TypedConstantKind.Error)
                    viewTypeSymbol = viewTypeArgument.Value.Value as INamedTypeSymbol;
            }

            // Try to get a view from naming convention
            if (viewTypeSymbol is null)
            {
                var expectedView = viewModelType.CalculateViewName();

                if (args.Views.FirstOrDefault(x => x.Identifier.Text == expectedView) is not { } viewDeclarationSyntax)
                    throw new ViewNotFoundException(viewModelType);

                viewTypeSymbol = args.GetDeclaredSymbol(viewDeclarationSyntax, cancellationToken);
            }

            if (viewTypeSymbol is null)
                throw new ViewNotFoundException(viewModelType);

            var viewModelTypesAndNamespaces = GetNameAndContainingTypesAndNamespaces(viewModelType);
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
        return "global::" + symbol.ToDisplayString(new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces));
    }

    static GenerateViewBindingArgs CreateViewBindingsArgs(Compilation compilation, IEnumerable<ClassDeclarationSyntax> viewModels)
    {

        string? shortestNamespace = null;
        var views = new List<ClassDeclarationSyntax>();

        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            var root = syntaxTree.GetRoot();

            foreach (var descendantNode in root.DescendantNodes())
            {
                if (descendantNode is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.Identifier.Text.EndsWith("View"))
                {
                    views.Add(classDeclarationSyntax);
                    continue;
                }

                if (descendantNode is not BaseNamespaceDeclarationSyntax baseNamespaceDeclarationSyntax)
                    continue;

                var @namespace = baseNamespaceDeclarationSyntax.Name.ToString();

                if (@namespace == "XamlGeneratedNamespace")
                    continue;

                if (shortestNamespace is null || shortestNamespace.Length > @namespace.Length)
                    shortestNamespace = @namespace;
            }
        }

        return new GenerateViewBindingArgs(compilation, shortestNamespace, viewModels, views);
    }
}