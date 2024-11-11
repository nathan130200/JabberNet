using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Features;

[XmppTag("session", Namespaces.Session)]
public class Session : Element
{
    public Session() : base("session", Namespaces.Session)
    {

    }
}