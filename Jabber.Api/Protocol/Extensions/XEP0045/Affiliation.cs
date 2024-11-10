using System.Globalization;
using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.XEP0045;

[XmppEnum]
public enum Affiliation
{
    [XmppEnumMember("none")]
    None,

    [XmppEnumMember("outcast")]
    Outcast,

    [XmppEnumMember("member")]
    Member,

    [XmppEnumMember("admin")]
    Admin,

    [XmppEnumMember("owner")]
    Owner,
}

[XmppTag("x", Namespaces.Muc)]
public class Muc : Element
{
    public Muc() : base("x", Namespaces.Muc)
    {

    }

    public string? Password
    {
        get => GetTag("password");
        set
        {
            RemoveTag("password");

            if (value != null)
                SetTag("password", value);
        }
    }
}

[XmppTag("history", Namespaces.Muc)]
public class History : Element
{
    public History() : base("history", Namespaces.Muc)
    {

    }

    public int? MaxChars
    {
        get => this.GetAttribute<int>("maxchars");
        set
        {
            if (!value.HasValue)
                RemoveAttribute("maxchars");
            else
                SetAttribute("maxchars", (int)value);
        }
    }

    public int? MaxStanzas
    {
        get => this.GetAttribute<int>("maxstanzas");
        set
        {
            if (!value.HasValue)
                RemoveAttribute("maxstanzas");
            else
                SetAttribute("maxstanzas", (int)value);
        }
    }

    public int? Seconds
    {
        get => this.GetAttribute<int>("seconds");
        set
        {
            if (!value.HasValue)
                RemoveAttribute("seconds");
            else
                SetAttribute("seconds", (int)value);
        }
    }

    static readonly string s_DateFormat = "yyyy-MM-dd'T'HH:mm:sszzz";

    public DateTime? Since
    {
        get
        {
            if (DateTime.TryParseExact(GetAttribute("since"), s_DateFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                return result;

            return default;
        }
        set
        {
            if (!value.HasValue)
                RemoveAttribute("since");
            else
            {
                SetAttribute("since", ((DateTime)value).ToString(s_DateFormat));
            }
        }
    }
}