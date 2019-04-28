using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PrintAPicStart
{
    class Helper
    {
        public static string FILENAME = "data.xml";
        public static void SerializeText(UserSettings obj)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(UserSettings));
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(FILENAME))
                {
                    xsSubmit.Serialize(writer, obj);
                    xml = sww.ToString(); // Your XML
                }
            }

        }

        public static UserSettings DeserializeText()
        {
            using (var stream = System.IO.File.OpenRead(FILENAME))
            {
                var serializer = new XmlSerializer(typeof(UserSettings));
                return serializer.Deserialize(stream) as UserSettings;
            }
        }

    }
}
