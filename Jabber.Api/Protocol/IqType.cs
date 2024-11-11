using Jabber.Attributes;

namespace Jabber.Protocol;

[XmppEnum]
public enum IqType
{
    [XmppMember("error")]
    Error,

    [XmppMember("get")]
    Get,

    [XmppMember("set")]
    Set,

    [XmppMember("result")]
    Result
}