namespace Jabber.Protocol.Base;

public sealed record Stream : Stanza
{
    public Stream(Stream other) : base(other)
    {

    }

    public Stream() : base("stream:stream", Namespaces.Stream)
    {

    }

    public string? Version
    {
        get => GetAttribute("version");
        set => SetAttribute("version", value);
    }
}
