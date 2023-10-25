namespace ChatGPTeamsAI.Data.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
internal sealed class MethodDescriptionAttribute : Attribute
{
    public string Description { get; }

    public string Category { get; }

    public MethodDescriptionAttribute(string category, string description)
    {
        Description = description;
        Category = category;
    }
}

// Custom attribute to describe a parameter
[AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
internal sealed class ParameterDescriptionAttribute : Attribute
{
    public string Description { get; }

    public ParameterDescriptionAttribute(string description)
    {
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal class ColumnNameAttribute : Attribute
{
    public string Name { get; set; }

    public ColumnNameAttribute() { }

    public ColumnNameAttribute(string name)
    {
        Name = name;
    }
}
