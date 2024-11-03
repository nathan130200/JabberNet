using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Sasl;

[XmppTag("auth", Namespaces.Sasl)]
public class Auth : Element
{
    public Auth(Auth other) : base(other)
    {
    }

    public Auth() : base("auth", Namespaces.Sasl)
    {

    }

    public Auth(string mechanismName) : this()
    {
        Mechanism = mechanismName;
    }

    public string Mechanism
    {
        get => GetAttribute("mechanism")!;
        set
        {
            ThrowHelper.ThrowIfNullOrWhiteSpace(value, nameof(Mechanism));
            SetAttribute("mechanism", value);
        }
    }
}
