﻿using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.XEP0004;

[XmppTag("x", Namespaces.DataForms)]
public class Form : Element
{
    public Form() : base("x", Namespaces.DataForms)
    {

    }

    public Form(FormType type) : this()
    {
        Type = type;
    }

    public FormType Type
    {
        get => XmppEnum.FromXml(GetAttribute("type"), FormType.Prompt);
        set
        {
            if (!Enum.IsDefined(value))
                throw new ArgumentOutOfRangeException(nameof(Type));

            SetAttribute("type", XmppEnum.ToXml(value));
        }
    }

    public IEnumerable<Item> Items
    {
        get => Children<Item>();
        set
        {
            Children<Item>().Remove();

            if (value?.Any() == true)
            {
                foreach (var item in value)
                    AddChild(item);
            }
        }
    }
}