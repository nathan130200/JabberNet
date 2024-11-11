using Jabber.Attributes;

namespace Jabber.Protocol.Extensions.XEP0045;

[XmppEnum]
public enum Affiliation
{
    [XmppMember("none")]
    None,

    [XmppMember("outcast")]
    Outcast,

    [XmppMember("member")]
    Member,

    [XmppMember("admin")]
    Admin,

    [XmppMember("owner")]
    Owner,
}
