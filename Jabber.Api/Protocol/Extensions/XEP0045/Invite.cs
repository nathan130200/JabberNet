using Jabber.Attributes;
using Jabber.Protocol.Base;

namespace Jabber.Protocol.Extensions.XEP0045;

[XmppTag("invite", Namespaces.MucUser)]
public class Invite : DirectionalElement
{
    public Invite() : base("invite", Namespaces.MucUser)
    {

    }

    public string? Reason
    {
        get => GetTag("reason");
        set
        {
            RemoveTag("reason");

            if (value != null)
                SetTag("reason", value);
        }
    }
}