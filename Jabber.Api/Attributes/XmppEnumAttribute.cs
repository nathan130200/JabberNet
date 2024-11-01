namespace Jabber.Attributes;

[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
public sealed class XmppEnumAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class XmppEnumMemberAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
