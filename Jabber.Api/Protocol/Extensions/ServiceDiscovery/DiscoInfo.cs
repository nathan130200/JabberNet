using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.ServiceDiscovery;

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
        get => Children().OfType<Identity>();
        set
        {
            Identities.ForEach(n => n.Remove());
            value?.ForEach(AddChild);
        }
    }

    public IEnumerable<Feature> Features
    {
        get => Children().OfType<Feature>();
        set
        {
            Features.ForEach(n => n.Remove());
            value?.ForEach(AddChild);
        }
    }
}
