﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Jabber;

[DebuggerDisplay("{ToString(),nq}")]
public record Jid

#if NET7_0_OR_GREATER
    : IParsable<Jid>
#endif

{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string? _local, _resource;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string _domain;

    public Jid(string jid)
    {
        var at = jid.IndexOf('@');
        var slash = jid.IndexOf('/');

        if (at > 0)
            _local = jid[0..at];

        if (slash == -1)
            _domain = jid[(at + 1)..];
        else
        {
            _domain = jid[(at + 1)..slash];
            _resource = jid[(slash + 1)..];
        }
    }

    public string? Local
    {
        get => _local;
        set => _local = value;
    }

    public string Domain
    {
        get => _domain;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (Uri.CheckHostName(value) == UriHostNameType.Unknown)
                    throw new InvalidOperationException();
            }

            _domain = value;
        }
    }

    public string? Resource
    {
        get => _resource;
        set => _resource = value;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (_local != null)
            sb.Append(_local).Append('@');

        if (_domain != null)
            sb.Append(_domain);

        if (_resource != null)
            sb.Append('/').Append(_resource);

        return sb.ToString();
    }

    public static implicit operator string?(Jid? jid) => jid?.ToString();

    public static implicit operator Jid?(string? jid)
    {
        if (jid == null)
            return null;

        return new(jid);
    }

    public bool IsBare => string.IsNullOrWhiteSpace(_resource);

    public Jid Bare => this with
    {
        _resource = null
    };

#if NET7_0_OR_GREATER

    public static Jid Parse(string s, IFormatProvider? provider)
    {
        ThrowHelper.ThrowIfNull(s);
        return new Jid(s);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Jid result)
    {
        try
        {
            result = Parse(s!, provider);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

#endif
}
