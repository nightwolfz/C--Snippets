using System.Collections.Generic;
using System.Xml;

namespace Assets
{
    static class XmlHelpers
    {
        public static int SetDefaultValue(XmlNode node, string attribute, int defaultValue = 0)
        {
            if (node.Attributes != null && node.Attributes[attribute] != null)
            {
                return int.Parse(node.Attributes[attribute].Value);
            }
            return defaultValue;
        }
        public static string SetDefaultValue(XmlNode node, string attribute, string defaultValue = "")
        {
            if (node.Attributes != null && node.Attributes[attribute] != null)
            {
                return node.Attributes[attribute].Value;
            }
            return defaultValue;
        }
    }

}
