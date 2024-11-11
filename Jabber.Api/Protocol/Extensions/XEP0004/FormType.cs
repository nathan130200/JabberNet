using Jabber.Attributes;

namespace Jabber.Protocol.Extensions.XEP0004;

[XmppEnum]
public enum FormType
{
    [XmppMember("cancel")]
    Cancel,

    [XmppMember("form")]
    Prompt,

    [XmppMember("result")]
    Result,

    [XmppMember("submit")]
    Submit,
}