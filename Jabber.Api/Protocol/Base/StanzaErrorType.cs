﻿using Jabber.Attributes;

namespace Jabber.Protocol.Base;

[XmppEnum]
public enum StanzaErrorType
{
    [XmppMember("auth")]
    Auth,

    [XmppMember("cancel")]
    Cancel,

    [XmppMember("continue")]
    Continue,

    [XmppMember("modify")]
    Modify,

    [XmppMember("wait")]
    Wait
}
