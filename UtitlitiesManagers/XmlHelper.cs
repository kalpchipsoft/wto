using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace UtilitiesManagers
{
    public class XmlHelper
    {
        public XmlDocument GetXml<T>(object o, string root)
        {
            //Blank Namespace
            XmlSerializerNamespaces Namespace = new XmlSerializerNamespaces();
            Namespace.Add(string.Empty, string.Empty);

            //Remove xml declaration
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Encoding = Encoding.UTF8;

            //Stream to hold the serialize xml
            StringWriter sw = new StringWriter();

            XmlWriter xw = XmlWriter.Create(sw, xws);

            //Create Serializer object for required Class
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(root));
            serializer.Serialize(xw, o, Namespace);

            //Load XML to document
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sw.ToString());
            return doc;
        }
    }
}
