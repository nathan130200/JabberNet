using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.ServiceDiscovery;

[XmppTag("query", Namespaces.DiscoItems)]
public class DiscoItems : Element
{
    public DiscoItems() : base("query", Namespaces.DiscoItems)
    {

    }

    public IEnumerable<Item> Items
    {
        get => Children().OfType<Item>();
        set
        {
            Items?.ForEach(n => n.Remove());
            value?.ForEach(AddChild);
        }
    }
}
