using System;
using Microsoft.CodeAnalysis;
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
        throw new NotImplementedException();
    }
}