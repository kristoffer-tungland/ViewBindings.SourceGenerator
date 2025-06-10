using System;

namespace ViewBindings.SourceGenerator.Contracts.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ViewBindingsNamespaceAttribute : Attribute
{
    public string Namespace { get; }

    public ViewBindingsNamespaceAttribute(string @namespace)
    {
        Namespace = @namespace;
    }
}
