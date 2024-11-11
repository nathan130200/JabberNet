using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Sasl;

[XmppTag("success", Namespaces.Sasl)]
public class Success : Element
{
    public Success() : base("success", Namespaces.Sasl)
    {

    }
}
