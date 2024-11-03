using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Tls;

[XmppTag("starttls", Namespaces.Tls)]
public class StartTls : Element
{
    public StartTls() : base("starttls", Namespaces.Tls)
    {

    }

    public StartTlsPolicy State
    {
        get
        {
            StartTlsPolicy result = 0;
            if (HasTag("required")) result |= StartTlsPolicy.Required | StartTlsPolicy.Offered;
            else if (HasTag("optional")) result |= StartTlsPolicy.Optional | StartTlsPolicy.Offered;
            return result;
        }
        set
        {
            RemoveTag("optional", Namespaces.Tls);
            RemoveTag("required", Namespaces.Tls);

            if (value.HasFlag(StartTlsPolicy.Offered))
            {
                if (value.HasFlag(StartTlsPolicy.Required))
                    SetTag("required", Namespaces.Tls);
                else
                    SetTag("optional", Namespaces.Tls);
            }
        }
    }
}
