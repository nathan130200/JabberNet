﻿using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.XEP0030;

[XmppTag("item", Namespaces.DiscoItems)]
public class Item : Element
{
    public Item() : base("item", Namespaces.DiscoItems)
    {

    }

    public Jid? Jid
    {
        get => GetAttribute("jid");
        set => SetAttribute("jid", value);
    }

    public string? Name
    {
        get => GetAttribute("name");
        set => SetAttribute("name", value);
    }

    public string? Node
    {
        get => GetAttribute("node");
        set => SetAttribute("node", value);
    }
}
