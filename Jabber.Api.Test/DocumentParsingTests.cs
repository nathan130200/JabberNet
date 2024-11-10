using Jabber.Dom;
using Jabber.Protocol;
using System.Text;

namespace Jabber.Test;

[TestClass]
public class DocumentParsingTests
{
    static readonly string SampleXml = @"<foo><bar xmlns='urn:mstest:app'><baz count='2' /></bar></foo>";

    [TestMethod]
    public void ParseFromString()
    {
        var doc = new Document();
        doc.Parse(SampleXml);

        Assert.IsNotNull(doc.RootElement);

        var root = doc.RootElement;
        Assert.AreEqual("foo", root.TagName);

        var child = root.FirstChild;
        Assert.IsNotNull(child);
        Assert.AreEqual("bar", child.TagName);
        Assert.AreEqual("urn:mstest:app", child.DefaultNamespace);

        var sub = child.FirstChild;
        Assert.IsNotNull(sub);
        Assert.AreEqual("2", sub.GetAttribute("count"));
    }

    [TestMethod]
    public void ParseFromFactory()
    {
        var xml = "<iq xmlns='jabber:client' type='result'><query xmlns='urn:cryonline:k01'><setserver master_node='localhost' /></query></iq>";

        var doc = new Document().Parse(xml);

        Assert.IsInstanceOfType<Iq>(doc.RootElement);

        var elem = doc.RootElement as Iq;
        Assert.IsNotNull(elem);

        Assert.AreEqual(IqType.Result, elem.Type);

        var query = elem.FirstChild;
        Assert.IsNotNull(query);

        Assert.AreEqual("query", query.TagName);
        Assert.AreEqual(Namespaces.CryOnline, query.Namespace);

        var child = query.FirstChild;
        Assert.IsNotNull(child);

        Assert.AreEqual("setserver", child.TagName);
        Assert.AreEqual(Namespaces.CryOnline, child.Namespace);
        Assert.AreEqual("localhost", child.GetAttribute("master_node"));
    }

    [TestMethod]
    public void ParseFromStream()
    {
        var doc = new Document();

        {
            var buffer = Encoding.UTF8.GetBytes(SampleXml);

            using (var stream = new MemoryStream(buffer))
                doc.Load(stream);
        }

        Assert.IsNotNull(doc.RootElement);

        var root = doc.RootElement;
        Assert.AreEqual("foo", root.TagName);

        var child = root.FirstChild;
        Assert.IsNotNull(child);
        Assert.AreEqual("bar", child.TagName);
        Assert.AreEqual("urn:mstest:app", child.DefaultNamespace);

        var sub = child.FirstChild;
        Assert.IsNotNull(sub);
        Assert.AreEqual("2", sub.GetAttribute("count"));
    }
}