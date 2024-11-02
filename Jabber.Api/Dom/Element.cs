using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Jabber.Dom;

[DebuggerDisplay("{StartTag(),nq}")]
[DebuggerTypeProxy(typeof(ElementTypeProxy))]
public record class Element
{
    class ElementTypeProxy
    {
        private readonly Element? _element;

        public ElementTypeProxy(Element? e)
            => _element = e;

        public string? TagName
        {
            get => _element?.TagName;
            set => _element!.TagName = value!;
        }

        public string? Namespace
        {
            get => _element?.Namespace;
            set => _element!.Namespace = value!;
        }

        public string? DefaultNamespace
        {
            get => _element?.GetNamespace();
            set => _element?.SetNamespace(value!);
        }

        public IReadOnlyList<Element>? Children => _element?.Children();
        public IReadOnlyDictionary<string, string>? Attributes => _element?.Attributes;
        public Element? FirstChild => _element?.FirstChild;
        public Element? LastChild => _element?.LastChild;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Element? _parent;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string _localName = default!;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string? _prefix;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private List<Element> _children;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Dictionary<string, string> _attributes;

    ~Element()
    {
        _localName = null!;
        _prefix = null!;

        if (_children != null)
        {
            foreach (var element in _children)
                element._parent = null;

            _children.Clear();
            _children = null!;
        }

        _attributes?.Clear();
        _attributes = null!;
    }

    public Element(string tagName, string? namespaceURI = default, object? value = default)
    {
        _children = new();
        _attributes = new();

        TagName = tagName;

        if (namespaceURI != null)
        {
            if (Prefix != null)
                SetNamespace(Prefix, namespaceURI);
            else
                SetNamespace(namespaceURI);
        }

        if (value != null)
            Value = Convert.ToString(value, CultureInfo.InvariantCulture);
    }

    public Element(Element parent, string tagName, string? xmlns = default, object? value = default) : this(tagName, xmlns, value)
    {
        ArgumentNullException.ThrowIfNull(parent);
        Parent = parent;
    }

    public Element(Element other)
    {
        _children = new();
        _attributes = new();
        _localName = other._localName;
        _prefix = other._prefix;

        foreach (var (key, value) in other.Attributes)
            _attributes[key] = value;

        lock (other._children)
        {
            foreach (var element in other.Children())
                _children.Add(element with { _parent = this });
        }

        Value = other.Value;
    }

    public bool IsRootElement
        => _parent == null;

    public string? Value
    {
        get;
        set;
    }

    public Element? Parent
    {
        get => _parent;
        set
        {
            _parent?.RemoveChild(this);
            value?.AddChild(this);
        }
    }

    public string? Prefix
    {
        get => _prefix;
        set => _prefix = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public string LocalName
    {
        get => _localName;
        set
        {
            ThrowHelper.ThrowIfNullOrWhiteSpace(value, nameof(LocalName));
            _localName = value;
        }
    }

    public string TagName
    {
        get => _prefix == null ? _localName : string.Concat(_prefix, ':', _localName);
        set
        {
            ThrowHelper.ThrowIfNullOrWhiteSpace(value, nameof(TagName));
            var ofs = value.IndexOf(':');

            if (ofs > 0)
                Prefix = value[0..ofs];

            LocalName = value[(ofs + 1)..];
        }
    }

    public void Remove()
        => _parent?.RemoveChild(this);

    public void AddChild(Element e)
    {
        if (e._parent != null)
            e = e with { _parent = this };

        lock (_children)
            _children.Add(e);
    }

    public void RemoveChild(Element e)
    {
        if (e._parent != this)
            return;

        lock (_children)
        {
            _children.Remove(e);
            e._parent = null;
        }
    }

    public void SetAttribute(string name, object? value, string? format = default, IFormatProvider? formatter = default)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(name);

        if (value is null)
            RemoveAttribute(name);
        else
        {
            formatter ??= CultureInfo.InvariantCulture;

            lock (_attributes)
            {
                string? rawValue;
                if (value is IFormattable fmt) rawValue = fmt.ToString(format, formatter);
                else if (value is IConvertible conv) rawValue = conv.ToString(formatter);
                else rawValue = Convert.ToString(value, formatter);
                _attributes[name] = rawValue ?? string.Empty;
            }
        }
    }

    public string? GetAttribute(string name)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(name);

        lock (_attributes)
            return _attributes.GetValueOrDefault(name);
    }

    public void RemoveAttribute(string name)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(name);

        lock (_attributes)
            _attributes.Remove(name);
    }

    public string? GetNamespace(string? prefix = default)
    {
        var result = GetAttribute(string.IsNullOrWhiteSpace(prefix) ? "xmlns" : $"xmlns:{prefix}");

        if (result != null)
            return result;

        return _parent?.GetNamespace(prefix);
    }

    public void SetNamespace(string? namespaceURI)
    {
        ArgumentNullException.ThrowIfNull(namespaceURI);
        SetAttribute("xmlns", namespaceURI);
    }

    public void SetNamespace(string prefix, string? namespaceURI)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(prefix);
        ArgumentNullException.ThrowIfNull(namespaceURI);
        SetAttribute($"xmlns:{prefix}", namespaceURI);
    }

    public Element? FirstChild
    {
        get
        {
            lock (_children)
                return _children.FirstOrDefault();
        }
    }

    public Element? LastChild
    {
        get
        {
            lock (_children)
                return _children.LastOrDefault();
        }
    }

    public string? Namespace
    {
        get => GetNamespace(Prefix);
        set
        {
            if (Prefix == null)
                SetNamespace(value);
            else
                SetNamespace(Prefix, value);
        }
    }

    public IReadOnlyDictionary<string, string> Attributes
    {
        get
        {
            KeyValuePair<string, string>[] result;

            lock (_attributes)
                result = _attributes.ToArray();

            return result.ToDictionary(x => x.Key, x => x.Value);
        }
    }

    public void WriteTo(TextWriter textWriter, XmlFormatting formatting = XmlFormatting.Default)
    {
        using var writer = Xml.CreateWriter(textWriter, formatting);
        Xml.WriteTree(this, writer, formatting);
    }

    public void Save(Stream stream, XmlFormatting formatting = XmlFormatting.Default)
    {
        using var writer = new StreamWriter(stream, Encoding.UTF8, -1, true);
        WriteTo(writer, formatting);
    }

    public string StartTag()
    {
        var sb = new StringBuilder($"<{Xml.EncodeName(TagName)}");

        foreach (var (key, value) in Attributes)
            sb.AppendFormat(" {0}=\"{1}\"", Xml.EncodeName(key), Xml.EscapeAttribute(value));

        return sb.Append('>').ToString();
    }

    public string EndTag()
        => $"</{Xml.EncodeName(TagName)}>";

    public override string ToString() => ToString(false);

    public string ToString(bool indented)
    {
        var sb = new StringBuilder();

        var formatting = XmlFormatting.Default;

        if (indented)
            formatting |= XmlFormatting.Indented;

        using (var sw = new StringWriter(sb))
            WriteTo(sw, formatting);

        return sb.ToString();
    }

    public IReadOnlyList<Element> Children()
    {
        Element[] result;

        lock (_children)
            result = _children.ToArray();

        return result.ToList();
    }

    public IEnumerable<Element> Children(string tagName, string? namespaceURI = default)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(tagName);
        return ChildrenInternal(tagName, namespaceURI, false);
    }

    IEnumerable<Element> ChildrenInternal(string tagName, string? ns, bool once)
    {
        lock (_children)
        {
            foreach (var element in _children)
            {
                if (element.TagName == tagName && (ns == null || ns == element.Namespace))
                {
                    yield return element;

                    if (once)
                        yield break;
                }
            }
        }
    }

    public Element? Child(string tagName, string? namespaceURI = default)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(tagName);
        return ChildrenInternal(tagName, namespaceURI, true).FirstOrDefault();
    }

    public bool HasTag(string tagName, string? namespaceURI = default)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(tagName);
        return ChildrenInternal(tagName, namespaceURI, true).Any();
    }

    public Element SetTag(string tagName, string? namespaceURI = default, object? value = default)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(tagName);
        var el = new Element(tagName, namespaceURI, value);
        AddChild(el);
        return el;
    }

    public void RemoveTag(string tagName, string? namespaceURI = default)
        => Child(tagName, namespaceURI)?.Remove();
}
