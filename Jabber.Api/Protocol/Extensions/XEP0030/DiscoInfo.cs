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
            Identities.Remove();
            value?.ForEach(AddChild);
        }
    }

    public IEnumerable<Feature> Features
    {
        get => Children<Feature>();
        set
        {
            Features.Remove();
            value?.ForEach(AddChild);
        }
    }
}
