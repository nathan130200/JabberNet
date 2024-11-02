namespace Jabber.Attributes;

[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
public sealed class XmppEnumAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class XmppEnumMemberAttribute : Attribute
{
    public string Name { get; }

    public XmppEnumMemberAttribute(string name)
    {
        Name = name;
    }
}
