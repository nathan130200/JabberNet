using Jabber.Attributes;

namespace Jabber.Protocol;

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