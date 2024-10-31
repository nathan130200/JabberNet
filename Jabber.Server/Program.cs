using Jabber;
using Jabber.Dom;

var el = new Element("stream:stream", Namespaces.Stream);
el.SetNamespace(Namespaces.Client);

var features = new Element("stream:features")
{
	Parent = el
};

_ = new Element(features, "bind", Namespaces.Bind);
_ = new Element(features, "session", Namespaces.Session);

using (var fs = File.Create("test.xml"))
{
	fs.SetLength(0);
	el.Save(fs, XmlFormatting.Indented);
}