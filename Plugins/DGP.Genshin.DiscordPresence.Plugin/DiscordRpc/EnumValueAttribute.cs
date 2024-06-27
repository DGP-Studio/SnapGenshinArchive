using System;

internal class EnumValueAttribute : Attribute
{
    public string Value { get; set; }
    public EnumValueAttribute(string value)
    {
        this.Value = value;
    }
}
