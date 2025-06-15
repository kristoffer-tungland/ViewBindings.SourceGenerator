using System;

namespace ViewBindings.SourceGenerator.Contracts.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ViewBindingAttribute : Attribute
{
    public Type? ViewType { get; set; }

    public ViewBindingAttribute()
    {
    }

    public ViewBindingAttribute(Type viewType)
    {
        ViewType = viewType;
    }
}