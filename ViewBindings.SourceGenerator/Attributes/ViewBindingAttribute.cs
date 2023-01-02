using System;

namespace ViewBindings.SourceGenerator.Attributes;

public class ViewBindingAttribute : Attribute
{
    public Type? ViewType { get; set; }
}