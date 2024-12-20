﻿using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.XEP0030;

[XmppTag("query", Namespaces.DiscoItems)]
public class DiscoItems : Element
{
    public DiscoItems() : base("query", Namespaces.DiscoItems)
    {

    }

    public IEnumerable<Item> Items
    {
        get => Children<Item>();
        set
        {
            Children<Item>()?.Remove();

            if (value?.Any() == true)
            {
                foreach (var item in value)
                    AddChild(item);
            }
        }
    }
}
