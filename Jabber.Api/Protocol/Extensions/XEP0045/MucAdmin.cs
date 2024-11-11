using Jabber.Attributes;
using Jabber.Dom;

namespace Jabber.Protocol.Extensions.XEP0045;

[XmppTag("query", Namespaces.MucAdmin)]
public class MucAdmin : Element
{
    public MucAdmin() : base("query", Namespaces.MucAdmin)
    {

    }

    public IEnumerable<Item> Items
    {
        get => Children<Item>();
        set
        {
            Children<Item>()?.Remove();

            if (value?.Any() == true)
            {
                foreach (var item in value)
                {
                    if (item != null)
                        AddChild(item);
                }
            }
        }
    }
}
