using Jabber.Attributes;
using Jabber.Collections;
using Jabber.Protocol;
using Jabber.Protocol.Base;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Jabber.Dom;

public static class ElementFactory
{
    static readonly ConcurrentDictionary<Type, IEnumerable<XmppTag>> s_ElementTypes = new();
    static readonly IEnumerable<XmppTag> s_Empty = Enumerable.Empty<XmppTag>();

    static ElementFactory()
    {
        RegisterElementsFromAssembly(typeof(ElementFactory).Assembly);
    }

    public static void RegisterElement<T>()
        => RegisterElement(typeof(T));

    public static void RegisterElement(Type type)
    {
        ThrowHelper.ThrowIfNull(type);

        var tags = from a in type.GetCustomAttributes<XmppTagAttribute>()
                   select new XmppTag(a.TagName, a.NamespaceURI);

        if (!tags.Any())
            return;

        if (!s_ElementTypes.TryGetValue(type, out var current))
            s_ElementTypes[type] = tags;
        else
            s_ElementTypes[type] = current.Concat(tags);
    }

    public static void RegisterElementsFromAssembly(Assembly assembly)
    {
        ThrowHelper.ThrowIfNull(assembly);

        var elements = from type in assembly.GetTypes()
                       where !type.IsAbstract && type.IsSubclassOf(typeof(Element))
                       let tags =
                            from attr in type.GetCustomAttributes<XmppTagAttribute>()
                            select new XmppTag(attr.TagName, attr.NamespaceURI)
                       where tags.Any()
                       select new { type, tags };

        foreach (var it in elements)
        {
            if (!s_ElementTypes.TryGetValue(it.type, out var current))
                s_ElementTypes[it.type] = it.tags;
            else
                s_ElementTypes[it.type] = current.Concat(it.tags);
        }
    }

    public static IEnumerable<XmppTag> LookupTagsFromType<T>() => LookupTagsFromType(typeof(T));

    public static IEnumerable<XmppTag> LookupTagsFromType(Type targetType)
        => s_ElementTypes.GetValueOrDefault(targetType) ?? s_Empty;

    static bool TryLookupSpecialElement(string tag, string? ns, [NotNullWhen(true)] out Type? result)
    {
        result = (tag, ns) switch
        {
            ("stream:stream", Namespaces.Stream) => typeof(StreamStream),
            ("iq", _) => typeof(Iq),
            //("message", _) => typeof(Message),
            //("presence", _) => typeof(Presence),
            _ => null
        };

        return result != null;
    }

    public static Type? LookupTypeFromTag(string tagName, string? namespaceURI)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(tagName);

        if (TryLookupSpecialElement(tagName, namespaceURI, out var result))
            return result;

        var find = new XmppTag(tagName, namespaceURI);
        return s_ElementTypes.FirstOrDefault(xe => xe.Value.Any(xt => xt == find)).Key;
    }

    public static Element CreateElement(string tagName, string? namespaceURI)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(tagName);

        Element result;

        var type = LookupTypeFromTag(tagName, namespaceURI);

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
