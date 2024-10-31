using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Jabber.Dom;

public record class Element
{
	private Element? _parent;
	private string _localName = default!;
	private string? _prefix;

	private readonly List<Element> _children;
	private readonly Dictionary<string, string> _attributes;

	public Element(string tagName, string? xmlns = default, object? value = default)
	{
		_children = [];
		_attributes = [];

		TagName = tagName;

		if (xmlns != null)
		{
			if (Prefix != null)
				SetNamespace(Prefix, xmlns);
			else
				SetNamespace(xmlns);
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
		_children = [];
		_attributes = [];
		_localName = other._localName;
		_prefix = other._prefix;

		lock (other._attributes)
		{
			foreach (var (key, value) in other._attributes)
				_attributes[key] = value;
		}

		lock (other._children)
		{
			foreach (var element in other._children)
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
			ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(LocalName));
			_localName = value;
		}
	}

	public string TagName
	{
		get => _prefix == null ? _localName : string.Concat(_prefix, ':', _localName);
		set
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(TagName));
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
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

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
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		lock (_attributes)
			return _attributes.GetValueOrDefault(name);
	}

	public void RemoveAttribute(string name)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

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
		ArgumentException.ThrowIfNullOrWhiteSpace(prefix);
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
				result = [.. _attributes];

			return result.ToDictionary(x => x.Key, x => x.Value).AsReadOnly();
		}
	}

	public IReadOnlyList<Element> Children
	{
		get
		{
			Element[] result;

			lock (_children)
				result = [.. _children];

			return result.ToList().AsReadOnly();
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
		=> $"<{Xml.EncodeName(TagName)}/>";

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
}
