using System.IO;
using System.Xml.Serialization;

namespace LootTracker
{
    public class XmlHandler
    {
        public void WriteXML(object Data)
        {
            //Initialize a new instance of an XMLSerializer.
            XmlSerializer writer = new XmlSerializer(Data.GetType());

            //Folder path to export the xml.
            var path = @"c:\users\ddenson\desktop\LootTracker.xml";

            //Create the file.
            FileStream file = File.Create(path);

            //Write the data to the file.
            writer.Serialize(file, Data);

            //Close the file.
            file.Close();
        }

        public LootBook ReadXML()
        {
            //Initialize a new instance of an XMLSerializer.
            XmlSerializer reader = new XmlSerializer(typeof(LootBook));

            //Read the file.
            StreamReader file = new System.IO.StreamReader(@"c:\users\ddenson\desktop\LootTracker.xml");

            //Deserialize the data.
            LootBook book = (LootBook)reader.Deserialize(file);

            //Close the file.
            file.Close();

            //Return the lootbook object.
            return book;
        }
    }
}
