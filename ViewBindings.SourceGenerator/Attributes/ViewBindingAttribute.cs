using System;

namespace ViewBindings.SourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ViewBindingAttribute : Attribute
{
    public Type? ViewType { get; set; }
}