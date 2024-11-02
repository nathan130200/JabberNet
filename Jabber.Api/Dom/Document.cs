using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Jabber.Dom;

public sealed class Document
{
    public bool? Standalone { get; set; } = default;
    public Encoding Encoding { get; set; } = Encoding.UTF8;

    public Element? RootElement
    {
        get;
        set;
    }

    public Document()
    {

    }

    public Document(Element rootElement)
        => RootElement = rootElement;

    public void Parse(string xml)
    {
        using (var reader = new StringReader(xml))
            ParseImpl(reader);
    }

    public void Load(string file, Encoding? encoding = default, int bufferSize = 4096)
    {
        encoding ??= Encoding.UTF8;

        if (bufferSize <= 0)
            bufferSize = 4096;

        using (var reader = new StreamReader(file, encoding, true, bufferSize))
            ParseImpl(reader);
    }

    public void Load(Stream stream, Encoding? encoding = default, int bufferSize = 4096, bool leaveOpen = true)
    {
        encoding ??= Encoding.UTF8;

        if (bufferSize <= 0)
            bufferSize = 4096;

        using (var reader = new StreamReader(stream, encoding, true, bufferSize, leaveOpen))
            ParseImpl(reader);
    }

    [ThreadStatic]
    static bool b_AllowXmlResolver = false;

    public static bool AllowXmlResolver
    {
        get => b_AllowXmlResolver;
        set => b_AllowXmlResolver = value;
    }

    public override string ToString()
        => ToString(false);

    public string ToString(bool indented)
    {
        var formatting = XmlFormatting.OmitDuplicatedNamespaces | XmlFormatting.CheckCharacters;

        if (indented)
            formatting |= XmlFormatting.Indented;

        return ToString(formatting);
    }

    public string ToString(XmlFormatting formatting)
    {
        var sb = new StringBuilder();

        using (var sw = new StringWriterWithEncoding(sb, Encoding))
        {
            using (var writer = Xml.CreateWriter(sw, formatting, Encoding))
            {
                var writeXmlDecl = !formatting.HasFlag(XmlFormatting.OmitXmlDeclaration);

                if (writeXmlDecl)
                {
                    if (Standalone.HasValue)
                        writer.WriteStartDocument((bool)Standalone);
                    else
                        writer.WriteStartDocument();
                }

                RootElement?.WriteTo(writer);

                if (writeXmlDecl)
                    writer.WriteEndDocument();
            }
        }

        return sb.ToString();
    }

    void ParseImpl(TextReader textReader)
    {
        ArgumentNullException.ThrowIfNull(textReader);

        var settings = new XmlReaderSettings
        {
            CheckCharacters = true,
            CloseInput = true,
            ConformanceLevel = ConformanceLevel.Fragment,
            DtdProcessing = DtdProcessing.Ignore,
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
            ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes,
        };

        if (!b_AllowXmlResolver)
            settings.XmlResolver = Xml.ThrowingResolver;

        Element? root = default,
            current = default;

        try
        {
            using (var reader = XmlReader.Create(textReader))
            {
                var info = (IXmlLineInfo)reader;

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                var elem = new Element(reader.Name, reader.NamespaceURI);

                                while (reader.MoveToNextAttribute())
                                    elem.SetAttribute(reader.Name, reader.Value);

                                reader.MoveToElement();

                                if (root == null)
                                    root = elem;

                                current?.AddChild(elem);
                                current = elem;
                            }
                            break;

                        case XmlNodeType.EndElement:
                            {
                                if (current == null)
                                    throw new XmlException("Unexcepted eng tag.", null, info.LineNumber, info.LinePosition);

                                var parent = current?.Parent;

                                if (parent == null)
                                    return;

                                current = parent;
                            }
                            break;

                        case XmlNodeType.SignificantWhitespace:
                        case XmlNodeType.Text:
                            {
                                if (current != null)
                                    current.Value += reader.Value;
                            }
                            break;

                        // skip unwanted whitespaces and PIs
                        case XmlNodeType.Whitespace:
                        case XmlNodeType.ProcessingInstruction:
                            break;

                        case XmlNodeType.XmlDeclaration:
                            {
                                var strEncoding = reader.GetAttribute("encoding");
                                Encoding = strEncoding == null ? Encoding.UTF8 : Encoding.GetEncoding(strEncoding);
                            }
                            break;

                        default:
                            throw new XmlException($"Unsupported XML token. ({reader.NodeType})", null, info.LineNumber, info.LinePosition);
                    }
                }
            }
        }
        finally
        {
            RootElement = root;
        }
    }
}


public sealed class StringWriterWithEncoding : StringWriter
{
    private readonly Encoding _encoding;

    public StringWriterWithEncoding(StringBuilder @out, Encoding encoding) : base(@out)
    {
        ArgumentNullException.ThrowIfNull(encoding);

        _encoding = encoding;
    }

    public override Encoding Encoding
        => _encoding;
}