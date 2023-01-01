using System;

namespace ViewBindings.SourceGenerator.Contracts.Attributes;

public class ViewBindingAttribute : Attribute
{
    public Type? ViewType { get; set; }
}