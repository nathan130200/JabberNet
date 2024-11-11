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
            if (value == null)
                RemoveTag("resource");
            else
                SetTag("resource", value: value);
        }
    }

    public Jid? Jid
    {
        get => GetTag("jid");
        set
        {
            if (value == null)
                RemoveTag("jid");
            else
                SetTag("jid", value: value.ToString());
        }
    }
}
