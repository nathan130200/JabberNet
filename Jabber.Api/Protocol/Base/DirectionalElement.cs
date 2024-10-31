using Jabber.Dom;
using System.Numerics;

namespace Jabber.Protocol.Base;

public abstract record DirectionalElement : Element
{
	public Jid? From
	{
		get => GetAttribute("from");
		set => SetAttribute("from", value);
	}

	public Jid? To
	{
		get => GetAttribute("from");
		set => SetAttribute("from", value);
	}

	public DirectionalElement(DirectionalElement other) : base(other)
	{

	}

	protected DirectionalElement(string tagName, string? xmlns = null, object? value = null) : base(tagName, xmlns, value)
	{
	}

	protected DirectionalElement(Element parent, string tagName, string? xmlns = null, object? value = null) : base(parent, tagName, xmlns, value)
	{
	}

	public void SwitchDirection()
	{
		var temp = To;
		To = From;
		From = temp;
	}
}

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

	public void GenerateId()
	{
		Id = Guid.NewGuid().ToString("D");
	}
}

//[XmppTag("stream:stream", Namespaces.Client)]
//[XmppTag("stream:stream", Namespaces.Accept)]
//[XmppTag("stream:stream", Namespaces.Connect)]
//[XmppTag("stream:stream", Namespaces.Server)]
public sealed class Stream : DirectionalElement
{

}