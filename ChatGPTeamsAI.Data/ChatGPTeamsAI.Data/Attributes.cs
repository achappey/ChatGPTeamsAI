namespace ChatGPTeamsAI.Data.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
internal sealed class MethodDescriptionAttribute : Attribute
{
    public string Description { get; }

    public string Category { get; }

    public string? ExportAction { get; }

    public MethodDescriptionAttribute(string category, string description, string? exportAction = null)
    {
        Description = description;
        Category = category;
        ExportAction = exportAction;
    }
}

// Custom attribute to describe a parameter
[AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
internal sealed class ParameterDescriptionAttribute : Attribute
{
    public string Description { get; }
    public bool? IsMultiline { get; }
    public bool? IsHidden { get; }

    public ParameterDescriptionAttribute(string description, bool isMultiline = false)
    {
        Description = description;
        IsMultiline = isMultiline;
    }

    public ParameterDescriptionAttribute(string description, bool isMultiline = false, bool isHidden = false)
    {
        Description = description;
        IsMultiline = isMultiline;
        IsHidden = isHidden;
    }

    public ParameterDescriptionAttribute(string description)
    {
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal class ListColumnAttribute : Attribute
{
    public ListColumnAttribute() { }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal class FormColumnAttribute : Attribute
{
    public string? Category { get; }
    
    public FormColumnAttribute() { }

    public FormColumnAttribute(string category)
    {
        Category = category;
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal class ImageColumnAttribute : Attribute
{
    public ImageColumnAttribute() { }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal class NewFormColumnAttribute : Attribute
{
    public NewFormColumnAttribute() { }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal class TitleColumnAttribute : Attribute
{
    public TitleColumnAttribute() { }
}


[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal class ActionColumnAttribute : Attribute
{
    public ActionColumnAttribute() { }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal class UpdatedColumnAttribute : Attribute
{
    public UpdatedColumnAttribute() { }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal class LinkColumnAttribute : Attribute
{
    public bool DocumentChat { get; } = false;
    public string? Category { get; }

    public LinkColumnAttribute() { }

    public LinkColumnAttribute(bool documentChat = false, string? category = null)
    {
        DocumentChat = documentChat;
        Category = category;
    }
}