using System.Text;
using Jabber.Dom;

namespace Jabber.Api.Test;

[TestClass]
public class DocumentParsingTest
{
    static readonly string SampleXml = @"
<foo>
    <bar xmlns='urn:mstest:app'>
        <baz count='2' />
    </bar>
</foo>";

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