﻿using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Sasl;

[XmppTag("mechanism", Namespaces.Sasl)]
public class Mechanism : Element
{
    public Mechanism() : base("mechanism", Namespaces.Sasl)
    {

    }

    public Mechanism(string mechanismName) : this()
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(mechanismName);
        Value = mechanismName;
    }
}
