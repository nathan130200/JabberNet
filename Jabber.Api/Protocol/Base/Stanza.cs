﻿using System.Diagnostics;
using Jabber.Dom;

namespace Jabber.Protocol.Base;

public abstract record Stanza : DirectionalElement
{
    protected Stanza(Stanza other) : base(other)
    {
    }

    protected Stanza(string tagName, string? xmlns = null, object? value = null) : base(tagName, xmlns, value)
    {

    }

    protected Stanza(Element parent, string tagName, string? xmlns = null, object? value = null) : base(parent, tagName, xmlns, value)
    {

    }

    public string? Id
    {
        get => GetAttribute("id");
        set => SetAttribute("id", value);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string? Type
    {
        get => GetAttribute("type");
        set => SetAttribute("type", value);
    }

    public string? Language
    {
        get => GetAttribute("xml:lang");
        set => SetAttribute("xml:lang", value);
    }

    public void GenerateId()
    {
        Id = Guid.NewGuid().ToString("D");
    }
}
