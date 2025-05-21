namespace Wen.Models.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DictAttribute(string category) : Attribute
{
    public string Category { get; set; } = category;
}
