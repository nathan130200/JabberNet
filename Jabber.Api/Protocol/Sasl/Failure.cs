using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Sasl;

[XmppTag("failure", Namespaces.Sasl)]
public class Failure : Element
{
    public Failure() : base("failure", Namespaces.Sasl)
    {

    }

    public Failure(FailureCondition? condition, string? text = default) : this()
    {
        Condition = condition;
        Text = text;
    }

    public FailureCondition? Condition
    {
        get
        {
            foreach (var (name, value) in XmppEnum.GetXmlMapping<FailureCondition>())
            {
                if (HasTag(name, Namespaces.Sasl))
                    return value;
            }

            return default;
        }
        set
        {
            foreach (var name in XmppEnum.GetXmlNames<FailureCondition>())
                RemoveTag(name, Namespaces.Sasl);

            if (value.HasValue)
                SetTag(XmppEnum.ToXml(value)!, Namespaces.Sasl);
        }
    }

    public string? Text
    {
        get => GetTag("text", Namespaces.Sasl);
        set
        {
            if (value == null)
                RemoveTag("text", Namespaces.Sasl);
            else
                SetTag("text", Namespaces.Sasl);
        }
    }
}
