namespace ViewBindings.SourceGenerator.Tests;

/// <summary>
/// Resource
/// https://andrewlock.net/creating-a-source-generator-part-2-testing-an-incremental-generator-with-snapshot-testing/
/// </summary>
[UsesVerify]
public class ViewBindingsSourceGeneratorTests
{
    [Fact]
    public Task GeneratesViewBindingsCorrectly()
    {
        const string viewModel = """
using ViewBindings.SourceGenerator.Attributes;

namespace ViewBindings.SourceGenerator.Demo.ViewModels;

[ViewBinding]
public class SecondViewModel
{

}
""";

        const string view = """
namespace ViewBindings.SourceGenerator.Demo.Views
{
    public class SecondView
    {
    }
}
""";
        return TestHelper.Verify(viewModel, view);
    }

    [Fact]
    public Task ViewTypeSpecified()
    {
        const string viewModel = """
using ViewBindings.SourceGenerator.Attributes;
using ViewBindings.SourceGenerator.Demo.Views;

namespace ViewBindings.SourceGenerator.Demo.ViewModels;

[ViewBinding(ViewType = typeof(FirstView))]
public class FirstViewModel
{
}
""";
        const string view = """
namespace ViewBindings.SourceGenerator.Demo.Views
{
    public class FirstView
    {
    }
}
""";

        return TestHelper.Verify(viewModel, view);
    }

    [Fact]
    public Task NoAttribute()
    {
        const string viewModel = """
using ViewBindings.SourceGenerator.Attributes;
using ViewBindings.SourceGenerator.Demo.Views;

namespace ViewBindings.SourceGenerator.Demo.ViewModels;

public class FirstViewModel
{
}
""";
        const string view = """
namespace ViewBindings.SourceGenerator.Demo.Views
{
    public class FirstView
    {
    }
}
""";

        return TestHelper.Verify(viewModel, view);
    }

    [Fact]
    public Task NoView()
    {
        const string viewModel = """
using ViewBindings.SourceGenerator.Attributes;
using ViewBindings.SourceGenerator.Demo.Views;

namespace ViewBindings.SourceGenerator.Demo.ViewModels;

[ViewBinding]
public class FirstViewModel
{
}
""";
        const string view = """
namespace ViewBindings.SourceGenerator.Demo.Views
{
    public class SecondView
    {
    }
}
""";

        return TestHelper.Verify(viewModel, view);
    }

    [Fact]
    public Task TwoViewModels()
    {
        const string viewModel = """
using ViewBindings.SourceGenerator.Attributes;
using ViewBindings.SourceGenerator.Demo.Views;

namespace ViewBindings.SourceGenerator.Demo.ViewModels;

[ViewBinding]
public class FirstViewModel
{
}

[ViewBinding]
public class SecondViewModel
{
}
""";
        const string view = """
namespace ViewBindings.SourceGenerator.Demo.Views
{
    public class FirstView
    {
    }

    public class SecondView
    {
    }
}
""";

        return TestHelper.Verify(viewModel, view);
    }
}