using Jabber.Attributes;
using Jabber.Protocol.Base;

namespace Jabber.Protocol;

[XmppTag("iq", Namespaces.Client)]
[XmppTag("iq", Namespaces.Accept)]
[XmppTag("iq", Namespaces.Connect)]
[XmppTag("iq", Namespaces.Server)]
public record class Iq : Stanza
{
    public Iq(Iq other) : base(other)
    {

    }

    public Iq() : base("iq")
    {

    }

    public Iq(IqType type) : this()
    {
        Type = type;
    }

    public new IqType? Type
    {
        get => XmppEnum.FromXmlOrDefault<IqType>(base.Type);
        set => base.Type = XmppEnum.ToXml(value);
    }
}

[XmppEnum]
public enum IqType
{
    [XmppEnumMember("error")]
    Error,

    [XmppEnumMember("get")]
    Get,

    [XmppEnumMember("set")]
    Set,

    [XmppEnumMember("result")]
    Result
}