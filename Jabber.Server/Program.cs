using System.Diagnostics;
using Jabber.Protocol;

namespace Jabber;

internal class Program
{
    static async Task Main(string[] args)
    {
        var stz = new Iq
        {
            Type = IqType.Result,
            Namespace = Namespaces.Client
        };

        var xml = stz.StartTag();

        Debugger.Break();
    }
}