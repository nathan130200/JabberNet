using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Jabber.Attributes;
using Jabber.Protocol;

namespace Jabber.Dom;

public static class ElementFactory
{
    static readonly ConcurrentDictionary<Type, IEnumerable<XmppTagInfo>> s_ElementTypes = new();
    static readonly IEnumerable<XmppTagInfo> s_Empty = Enumerable.Empty<XmppTagInfo>();

    static ElementFactory()
    {
        RegisterElementsFromAssembly(typeof(ElementFactory).Assembly);
    }

    public static void RegisterElementsFromAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        var elements = from type in assembly.GetTypes()
                       where !type.IsAbstract && type.IsSubclassOf(typeof(Element))
                       let attributes =
                            from attr in type.GetCustomAttributes<XmppTagAttribute>()
                            select new XmppTagInfo(attr.TagName, attr.NamespaceURI)
                       where attributes.Any()
                       select new { type, attributes };

        foreach (var element in elements)
            s_ElementTypes[element.type] = element.attributes;
    }

    public static IEnumerable<XmppTagInfo> GetTagsFromType(Type targetType)
        => s_ElementTypes.GetValueOrDefault(targetType) ?? s_Empty;

    static bool GetSpecialElementType(string tag, string? ns, [NotNullWhen(true)] out Type? result)
    {
        result = (tag, ns) switch
        {
            ("stream:stream", Namespaces.Stream) => typeof(Protocol.Base.Stream),
            ("iq", _) => typeof(Iq),
            //("message", _) => typeof(Message),
            //("presence", _) => typeof(Presence),
            _ => null
        };

        return result != null;
    }

    public static Type? GetTypeFromTag(string tagName, string? namespaceURI)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(tagName);

        if (GetSpecialElementType(tagName, namespaceURI, out var result))
            return result;

        var search = new XmppTagInfo(tagName, namespaceURI);
        return s_ElementTypes.FirstOrDefault(x => x.Value.Any(s => s == search)).Key;
    }

    public static Element CreateElement(string tagName, string? namespaceURI)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(tagName);

        Element result;

        var type = GetTypeFromTag(tagName, namespaceURI);

        if (type == null)
            result = new Element(tagName, namespaceURI);
        else
        {
            if (Activator.CreateInstance(type) is Element elem)
                result = elem;
            else
                result = new Element(tagName, namespaceURI);
        }

        return result;
    }
}

[DebuggerDisplay("Name={TagName}; Namespace={NamespaceURI}")]
public readonly struct XmppTagInfo : IEquatable<XmppTagInfo>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string TagName { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string? NamespaceURI { get; }

    public XmppTagInfo(string tagName, string? namespaceURI)
    {
        TagName = tagName;
        NamespaceURI = namespaceURI;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine
        (
            TagName?.GetHashCode() ?? 0,
            NamespaceURI?.GetHashCode() ?? 0
        );
    }

    public override bool Equals(object? obj)
        => obj is XmppTagInfo other && Equals(other);

    public bool Equals(XmppTagInfo other)
    {
        return string.Equals(TagName, other.TagName, StringComparison.Ordinal)
            && string.Equals(NamespaceURI, other.NamespaceURI, StringComparison.Ordinal);
    }

    public static bool operator ==(XmppTagInfo left, XmppTagInfo right) => left.Equals(right);
    public static bool operator !=(XmppTagInfo left, XmppTagInfo right) => !(left == right);
}
