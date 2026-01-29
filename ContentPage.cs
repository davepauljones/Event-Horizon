using System.Collections.Generic;
using System.Xml.Serialization;

namespace Event_Horizon
{
    [XmlRoot("ContentPage")]
    public class ContentPage
    {
        public string Title { get; set; }
        public Body Body { get; set; }

        // Optional for quiz pages
        public Question Question { get; set; }
    }

    public class Body
    {
        [XmlElement("Paragraph")]
        public List<string> Paragraphs { get; set; } = new List<string>();
    }

    public class Question
    {
        [XmlElement("Option")]
        public List<Option> Options { get; set; } = new List<Option>();
    }

    public class Option
    {
        [XmlText]
        public string Text { get; set; }

        [XmlAttribute("Correct")]
        public bool Correct { get; set; }
    }
}
