using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Sasl;

[XmppTag("challenge", Namespaces.Sasl)]
public sealed class Challenge : Element
{
    public Challenge() : base("challenge", Namespaces.Sasl)
    {

    }
}
