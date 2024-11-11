﻿using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Tls;

[XmppTag("starttls", Namespaces.Tls)]
public class StartTls : Element
{
    public StartTls() : base("starttls", Namespaces.Tls)
    {

    }

    public StartTlsPolicy Policy
    {
        get
        {
            StartTlsPolicy result;

            if (HasTag("required", Namespaces.Tls))
                result = StartTlsPolicy.Required;
            else
                result = StartTlsPolicy.Optional;

            return result;
        }
        set
        {
            RemoveTag("optional");
            RemoveTag("required");

            if (value == StartTlsPolicy.Required)
                SetTag("required", Namespaces.Tls);
            else
                SetTag("optional", Namespaces.Tls);
        }
    }
}
