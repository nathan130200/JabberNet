using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.XEP0030;

[XmppTag("query", Namespaces.DiscoInfo)]
public class DiscoInfo : Element
{
    public DiscoInfo() : base("query", Namespaces.DiscoInfo)
    {

    }

    public string? Node
    {
        get => GetAttribute("node");
        set => SetAttribute("node", value);
    }

    public IEnumerable<Identity> Identities
    {
        get => Children<Identity>();
        set
        {
            Children<Identity>().Remove();

            if (value.Any() == true)
            {
                foreach (var item in value)
                    AddChild(item);
            }
        }
    }

    public IEnumerable<Feature> Features
    {
        get => Children<Feature>();
        set
        {
            Children<Feature>().Remove();

            if (value.Any() == true)
            {
                foreach (var item in value)
                    AddChild(item);
            }
        }
    }
}
