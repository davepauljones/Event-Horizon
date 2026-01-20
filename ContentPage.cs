using System.Collections.Generic;
using System.Xml.Serialization;

namespace Event_Horizon
{
    [XmlRoot("ContentPage")]
    public class ContentPage
    {
        public string Title { get; set; }
        public Body Body { get; set; }
    }

    public class Body
    {
        [XmlElement("Paragraph")]
        public List<string> Paragraphs { get; set; }
    }
}
