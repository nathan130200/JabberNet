using Jabber.Dom;
using System.Security;
using System.Text;
using System.Web;
using System.Xml;

namespace Jabber;

public static class Xml
{
    [ThreadStatic]
    private static string? s_IndentChars;

    [ThreadStatic]
    private static string? s_NewLineChars;

    public static string NewLineChars
    {
        get
        {
            s_NewLineChars ??= "\n";
            return s_NewLineChars;
        }
    }

    public static string IndentChars
    {
        get
        {
            s_IndentChars ??= "  ";
            return s_IndentChars;
        }
    }

    public static string? EncodeName(string? s)
        => XmlConvert.EncodeName(s);

    public static string? Escape(string? s)
        => SecurityElement.Escape(s);

    public static string? EscapeAttribute(string? s)
        => HttpUtility.HtmlAttributeEncode(s);

    public static string? Unescape(string? s)
        => HttpUtility.HtmlDecode(s);


    internal static XmlWriter CreateWriter(TextWriter textWriter, XmlFormatting formatting)
    {
        var isFragment = formatting.HasFlag(XmlFormatting.OmitXmlDeclaration);

        return XmlWriter.Create(textWriter, new XmlWriterSettings()
        {
            Indent = formatting.HasFlag(XmlFormatting.Indented),
            IndentChars = IndentChars,
            ConformanceLevel = isFragment ? ConformanceLevel.Fragment : ConformanceLevel.Document,
            CloseOutput = false,
            Encoding = Encoding.UTF8,
            NamespaceHandling = formatting.HasFlag(XmlFormatting.OmitDuplicatedNamespaces)
                ? NamespaceHandling.OmitDuplicates
                : NamespaceHandling.Default,

            OmitXmlDeclaration = formatting.HasFlag(XmlFormatting.OmitXmlDeclaration),
            NewLineChars = NewLineChars,
            NewLineOnAttributes = formatting.HasFlag(XmlFormatting.NewLineOnAttributes),
            DoNotEscapeUriAttributes = formatting.HasFlag(XmlFormatting.DoNotEscapeUriAttributes),
            CheckCharacters = formatting.HasFlag(XmlFormatting.CheckCharacters),
        });
    }

    internal static void WriteTree(Element e, XmlWriter xw, XmlFormatting fmt)
    {
        var skipAttribute = e.Prefix == null ? "xmlns" : $"xmlns:{e.Prefix}";
        xw.WriteStartElement(e.Prefix, e.LocalName, e.Namespace);

        foreach (var (key, value) in e.Attributes)
        {
            if (key == skipAttribute) continue;

            var ofs = key.IndexOf(':');
            var prefix = ofs > 0 ? key[0..ofs] : null;
            var localName = key[(ofs + 1)..];

            if (prefix == null)
                xw.WriteAttributeString(localName, value);
            else
            {
                xw.WriteAttributeString(localName, prefix switch
                {
                    "xml" => Namespaces.Xml,
                    "xmlns" => Namespaces.Xmlns,
                    _ => e.GetNamespace(prefix) ?? string.Empty
                }, value);
            }
        }

        if (e.Value != null)
            xw.WriteString(e.Value);

        foreach (var child in e.Children())
            WriteTree(child, xw, fmt);

        xw.WriteEndElement();
    }
}
