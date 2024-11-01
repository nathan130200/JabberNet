using Jabber.Dom;
using Jabber.Protocol.Tls;

namespace Jabber.Protocol.Base;

public sealed record StreamFeatures : Element
{
    public override string ToString() => base.ToString(false);

    public StreamFeatures(StreamFeatures other) : base(other)
    {

    }

    public StreamFeatures() : base("stream:features", Namespaces.Stream)
    {

    }

    public TlsPolicy StartTls
    {
        get
        {
            TlsPolicy result = 0;

            var tag = Child("starttls", Namespaces.Tls);

            if (tag != null)
            {
                result |= TlsPolicy.Offered;

                if (tag.HasTag("required"))
                    result |= TlsPolicy.Required;
                else if (tag.HasTag("optional"))
                    result |= TlsPolicy.Optional;
            }

            return result;
        }
        set
        {
            Child("starttls", Namespaces.Tls)?.Remove();

            if (value.HasFlag(TlsPolicy.Offered))
            {
                var el = SetTag("starttls", Namespaces.Tls);

                if (value.HasFlag(TlsPolicy.Required))
                    el.SetTag("required");
                else if (value.HasFlag(TlsPolicy.Optional))
                    el.SetTag("optional");
            }
        }
    }

    public bool SupportBind
    {
        get => HasTag("bind", Namespaces.Bind);
        set
        {
            RemoveTag("bind", Namespaces.Bind);

            if (value)
                SetTag("bind", Namespaces.Bind);
        }
    }
    public bool SupportSession
    {
        get => HasTag("session", Namespaces.Session);
        set
        {
            RemoveTag("session", Namespaces.Session);

            if (value)
                SetTag("session", Namespaces.Session);
        }
    }
}
