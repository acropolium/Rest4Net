using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Rest4Net.Parsers
{
    internal class ParserXml : BaseSerializer<XmlNode>
    {
        public ParserXml(RestApiSerializableAttribute attribute)
            : base(attribute) { }

        protected override XmlNode ParseContent(byte[] content)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(Encoding.UTF8.GetString(content));
            return xDoc.DocumentElement;
        }

        protected override XmlNode FindSubLeaf(XmlNode parent, string key)
        {
            return
                parent.ChildNodes.OfType<XmlElement>().FirstOrDefault(
                    l =>
                    Attribute.IgnoreMemberCase
                        ? key.ToLower().Equals(GetRealName(l.Name.ToLower()))
                        : key.Equals(GetRealName(l.Name)));
        }

        protected override bool LeafHasChildren(XmlNode leaf)
        {
            return !((leaf.ChildNodes.Count == 0) || (leaf.ChildNodes.Count == 1 && leaf.ChildNodes[0] is XmlText));
        }

        protected override string GetValue(XmlNode leaf)
        {
            return leaf.ChildNodes[0].Value;
        }

        protected override IEnumerable<XmlNode> GetArray(XmlNode leaf)
        {
            return leaf.ChildNodes.OfType<XmlElement>();
        }
    }
}
