using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GmxVCount.Model
{
    [DataContract]
    [Serializable]
    public abstract class BaseMetrics
    {
        public XDocument ToXDocument()
        {
            var serializer = new XmlSerializer(GetType());
            var doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                serializer.Serialize(writer,this);
            }
            return doc;
        }

        public static T From<T>(string xml) where T : BaseMetrics
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var textReader = new StringReader(xml))
            {
                return serializer.Deserialize(textReader) as T;
            }
            
        }
        public void Save(Stream stream)
        {
            var doc = ToXDocument();
            doc.Save(stream);
        }

        public void Save(string path)
        {
            var doc = ToXDocument();
            doc.Save(path);
        }

        [XmlAttribute("version")]
        public string Version { get; set; }

        public const string DefaultVersion = "2.0";

        [XmlAttribute("source-language")]
        public string SourceLanguage { get; set; }

        [XmlAttribute("tool-name")]
        public string ToolName { get; set; }

        [XmlAttribute("tool-version")]
        public string ToolVersion { get; set; }

        private XmlSerializerNamespaces _namespaces;

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces NameSpaces
        {
            get { return _namespaces; }
        }

        protected BaseMetrics()
        {
            Version = DefaultVersion;
            _namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName(String.Empty, "urn:lisa-metrics-tags")
            });
        }
    }
}
