using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.XEP0030;

[XmppTag("identity", Namespaces.DiscoInfo)]
public class Identity : Element
{
    public Identity() : base("identity", Namespaces.DiscoInfo)
    {

    }

    public Identity(string category, string name, string? type = default) : this()
    {
        Category = category;
        Name = name;
        Type = type;
    }

    public string? Category
    {
        get => GetAttribute("category");
        set => SetAttribute("category", value);
    }

    public string? Name
    {
        get => GetAttribute("name");
        set => SetAttribute("name", value);
    }

    public string? Type
    {
        get => GetAttribute("type");
        set => SetAttribute("type", value);
    }
}