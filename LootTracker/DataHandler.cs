using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace LootTracker
{
    public class DataHandler
    {
        public void WriteData(object Data)
        {

            SaveFileDialog filepicker = new SaveFileDialog();
            filepicker.ShowDialog();
            FileStream fs = new FileStream(filepicker.FileName, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, Data);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        public LootBook ReadData()
        {

            OpenFileDialog filepicker = new OpenFileDialog();
            filepicker.ShowDialog();
            LootBook book = null;
            if (File.Exists(filepicker.FileName))
            {
                FileStream fs = new FileStream(filepicker.FileName, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                book = (LootBook)formatter.Deserialize(fs);
            }
            return book;
        }
    }
}
