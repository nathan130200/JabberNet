namespace Jabber.Protocol.Tls;

[Flags]
public enum TlsPolicy
{
    None,
    Offered = 1 << 1,
    Optional = 1 << 2,
    Required = 1 << 3
}