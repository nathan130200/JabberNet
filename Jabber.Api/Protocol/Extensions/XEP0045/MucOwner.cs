﻿using Jabber.Attributes;
using Jabber.Dom;
using Jabber.Protocol.Extensions.XEP0004;

namespace Jabber.Protocol.Extensions.XEP0045;

[XmppTag("query", Namespaces.MucOwner)]
public class MucOwner : Element
{
    public MucOwner() : base("query", Namespaces.MucOwner)
    {

    }

    public Form? Form
    {
        get => Child<Form>();
        set
        {
            Child<Form>()?.Remove();

            if (value != null)
                AddChild(value);
        }
    }

    public Destroy? Destroy
    {
        get => Child<Destroy>();
        set
        {
            Child<Destroy>()?.Remove();

            if (value != null)
                AddChild(value);
        }
    }
}
