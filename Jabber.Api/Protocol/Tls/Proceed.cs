using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Tls;

[XmppTag("proceed", Namespaces.Tls)]
public class Proceed : Element
{
    public Proceed() : base("proceed", Namespaces.Tls)
    {

    }
}
