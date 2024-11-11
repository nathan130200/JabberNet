using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Features;

[XmppTag("bind", Namespaces.Bind)]
public class Bind : Element
{
    public Bind() : base("bind", Namespaces.Bind)
    {

    }

    public Bind(string resource) : this()
        => Resource = resource;

    public Bind(Jid jid) : this()
        => Jid = jid;

    public string? Resource
    {
        get => GetTag("resource");
        set
        {
            RemoveTag("resource");

            if (value != null)
                SetTag("resource", value: value);
        }
    }

    public Jid? Jid
    {
        get => GetTag("jid");
        set
        {
            RemoveTag("jid");

            if (value != null)
                SetTag("jid", value: value.ToString());
        }
    }
}
