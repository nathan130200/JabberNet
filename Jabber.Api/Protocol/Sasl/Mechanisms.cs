using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Sasl;

[XmppTag("mechanisms", Namespaces.Sasl)]
public class Mechanisms : Element
{
    public Mechanisms(Mechanisms other) : base(other)
    {

    }

    public Mechanisms() : base("mechanisms", Namespaces.Sasl)
    {

    }

    public IEnumerable<Mechanisms> SupportedMechanisms
    {
        get => Children<Mechanisms>();
        set
        {
            ThrowHelper.ThrowIfNull(value);

            Children<Mechanisms>()?.Remove();

            if (value?.Any() == true)
            {
                foreach (var item in value)
                    AddChild(item);
            }
        }
    }

    public void AddMechanism(string name)
        => AddChild(new Mechanism(name));
}
