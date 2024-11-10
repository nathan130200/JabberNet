using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Tls;

[XmppTag("starttls", Namespaces.Tls)]
public class StartTls : Element
{
    public StartTls() : base("starttls", Namespaces.Tls)
    {

    }

    public StartTlsPolicy Policy
    {
        get
        {
            StartTlsPolicy result;

            if (HasTag("required"))
                result = StartTlsPolicy.Required;
            else
                result = StartTlsPolicy.Optional;

            return result;
        }
        set
        {
            ClearChildren();

            if (value.HasFlag(StartTlsPolicy.Required))
                SetTag("required", Namespaces.Tls);
            else
                SetTag("optional", Namespaces.Tls);
        }
    }
}
