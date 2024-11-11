﻿using Jabber.Dom;
using System.Runtime.CompilerServices;

namespace Jabber.Protocol.Base;

public abstract class DirectionalElement : Element
{
    protected DirectionalElement(DirectionalElement other) : base(other)
    {
    }

    protected DirectionalElement(string tagName, string? namespaceURI = null, object? value = null) : base(tagName, namespaceURI, value)
    {
    }

    public Jid? From
    {
        get => GetAttribute("from");
        set => SetAttribute("from", value);
    }

    public Jid? To
    {
        get => GetAttribute("to");
        set => SetAttribute("to", value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SwitchDirection()
        => (From, To) = (To, From);
}
