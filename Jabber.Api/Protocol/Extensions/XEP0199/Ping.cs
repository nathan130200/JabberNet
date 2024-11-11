using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.XEP0199;

[XmppTag("ping", "urn:xmpp:ping")]
public class Ping : Element
{
    public Ping() : base("ping", Namespaces.Ping)
    {

    }
}