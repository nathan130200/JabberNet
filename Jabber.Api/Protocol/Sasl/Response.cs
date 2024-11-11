using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Sasl;

[XmppTag("response", "urn:ietf:params:xml:ns:xmpp-sasl")]
public class Response : Element
{
    public Response() : base("response", Namespaces.Sasl)
    {

    }
}