using System.IO;
using System.Xml.Serialization;

namespace Event_Horizon
{
    public static class ContentLoader
    {
        public static ContentPage Load(string xmlPath)
        {
            var serializer = new XmlSerializer(typeof(ContentPage));

            using (var stream = File.OpenRead(xmlPath))
            {
                return (ContentPage)serializer.Deserialize(stream);
            }
        }
    }
}