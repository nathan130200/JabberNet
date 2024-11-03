using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.ServiceDiscovery;

[XmppTag("feature", Namespaces.DiscoInfo)]
public class Identity : Element
{
    public Identity() : base("feature", Namespaces.DiscoInfo)
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