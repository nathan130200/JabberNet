﻿using System.Collections;

namespace Jabber.Collections;

public sealed class BareJidComparer : IComparer, IComparer<Jid>
{
    public static BareJidComparer Shared { get; } = new();

    BareJidComparer()
    {

    }

    public int Compare(object? x, object? y) => Compare(x as Jid, y as Jid);

    public int Compare(Jid? x, Jid? y)
    {
        if (x == null && y == null) return 0;
        else if (x == null) return -1;
        else if (y == null) return 1;

        var result = string.Compare(x.Local, y.Local, StringComparison.OrdinalIgnoreCase);

        if (result != 0)
            return result;

        return string.Compare(x.Domain, y.Domain, StringComparison.OrdinalIgnoreCase);
    }
}
