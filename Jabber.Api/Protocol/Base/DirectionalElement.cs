using System.Runtime.CompilerServices;
using Jabber.Dom;

namespace Jabber.Protocol.Base;

public abstract record DirectionalElement : Element
{
    protected DirectionalElement(DirectionalElement other) : base(other)
    {
    }

    protected DirectionalElement(string tagName, string? xmlns = null, object? value = null) : base(tagName, xmlns, value)
    {
    }

    protected DirectionalElement(Element parent, string tagName, string? xmlns = null, object? value = null) : base(parent, tagName, xmlns, value)
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
