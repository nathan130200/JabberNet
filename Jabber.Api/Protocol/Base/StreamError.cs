using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Base;

[XmppTag("stream:error", Namespaces.Stream)]
public class StreamError : Element
{
    public StreamError() : base("stream:error", Namespaces.Stream)
    {

    }

    public StreamError(StreamErrorCondition? condition) : this()
    {
        Condition = condition;
    }

    public StreamErrorCondition? Condition
    {
        get
        {
            foreach (var (name, value) in XmppEnum.GetXmlMapping<StreamErrorCondition>())
            {
                if (HasTag(name))
                    return value;
            }

            return default;
        }
        set
        {
            foreach (var name in XmppEnum.GetXmlNames<StreamErrorCondition>())
                RemoveTag(name);

            if (value.HasValue)
            {
                var name = XmppEnum.ToXml((StreamErrorCondition)value)!;
                SetTag(name);
            }
        }
    }

    public string? Text
    {
        get => GetTag("text", Namespaces.Streams);
        set
        {
            if (value == null)
                RemoveTag("text", Namespaces.Streams);
            else
                SetTag("text", Namespaces.Streams, value);
        }
    }
}
