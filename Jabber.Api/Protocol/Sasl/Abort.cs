using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Sasl;

[XmppTag("abort", Namespaces.Sasl)]
public class Abort : Element
{
    public Abort() : base("abort", Namespaces.Sasl)
    {

    }
}
