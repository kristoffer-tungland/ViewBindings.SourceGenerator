using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ViewBindings.SourceGenerator.Tests;

public static class TestHelper
{
    public static Task Verify(string viewModel, string view)
    {
        // Parse the provided string into a C# syntax tree
        var viewModelSyntaxTree = CSharpSyntaxTree.ParseText(viewModel);
        var viewSyntaxTree = CSharpSyntaxTree.ParseText(view);

        // Create a Roslyn compilation for the syntax tree.
        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: new[] { viewModelSyntaxTree, viewSyntaxTree, GenerateNameSpaceNode() });

        // Create an instance of our EnumGenerator incremental source generator
        var generator = new ViewBindingsSourceGenerator();

        // The GeneratorDriver is used to run our generator against a compilation
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the source generator!
        driver = driver.RunGenerators(compilation);

        // Use verify to snapshot test the source generator output!
        return Verifier.Verify(driver).UseDirectory("Snapshots");
    }

    static SyntaxTree GenerateNameSpaceNode()
    {
        return CSharpSyntaxTree.ParseText("""
namespace ViewBindings.SourceGenerator.Demo
{
    public class Class1
    {
    }
}
""");
    }
}