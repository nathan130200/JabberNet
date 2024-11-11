using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.XEP0045;

[XmppTag("x", Namespaces.Muc)]
public class Muc : Element
{
    public Muc() : base("x", Namespaces.Muc)
    {

    }

    public string? Password
    {
        get => GetTag("password");
        set
        {
            RemoveTag("password");

            if (value != null)
                SetTag("password", value);
        }
    }
}
