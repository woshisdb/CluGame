using System;

[AttributeUsage(AttributeTargets.Class)]
public class GptResponseAttribute : Attribute
{
    public string Description;
    public GptResponseAttribute(string description)
    {
        Description = description;
    }
}