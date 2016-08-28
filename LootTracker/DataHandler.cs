using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace LootTracker
{
    public class DataHandler
    {
        //Fields.
        public string filepath;

        //WriteData method takes data object and serializes to a binary format.
        //Saves output to file path selected by user.
        public void WriteData(object Data)
        {
            FileStream filestream;

            //Instantiate a new instance of the SaveFileDialog.
            SaveFileDialog filepicker = new SaveFileDialog();
            filepicker.Filter = "Data Files(*.dat)|*.DAT";

            //Display the filepicker to the user.
            filepicker.ShowDialog();

            //Start a new filestream using the selected file path.
            if (string.IsNullOrEmpty(filepicker.FileName))
            {
                return;
            }
            else
            {
                filestream = new FileStream(filepicker.FileName, FileMode.Create);
            }
           

            filepath = filepicker.FileName;

            //instantiate a formatter object to serialize the data.
            BinaryFormatter formatter = new BinaryFormatter();

            //Try Catch to capture errors.
            try
            {
                //Serialize the data.
                formatter.Serialize(filestream, Data);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                //throw;
            }
            finally
            {
                //close the filestream.
                filestream.Close();
                filepicker.Dispose();
                filestream.Dispose();
            }
        }
        
        public void WriteData(bool SaveOnly, object Data, string Path)
        {
            //Start a new filestream using the selected file path.
            FileStream filestream = new FileStream(Path, FileMode.Create);

            //instantiate a formatter object to serialize the data.
            BinaryFormatter formatter = new BinaryFormatter();

            //Try Catch to capture errors.
            try
            {
                //Serialize the data.
                formatter.Serialize(filestream, Data);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                //throw;
            }
            finally
            {
                //close the filestream.
                filestream.Close();
                filestream.Dispose();
            }

        }

        //ReadData method reads a .dat file and deserializes the data back into a LootBook object.
        public LootBook ReadData()
        {
            OpenFileDialog filepicker = new OpenFileDialog();
            filepicker.Filter = "Data Files(*.dat)|*.DAT";

            //Prompt the user.
            filepicker.ShowDialog();

            LootBook book;

            if (!File.Exists(filepicker.FileName))
            {
                throw new FileLoadException();
            }
            else
            {
                filepath = filepicker.FileName;
                FileStream fs = new FileStream(filepicker.FileName, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                book = (LootBook)formatter.Deserialize(fs);
                filepicker.Dispose();
                return book;
            }
        }
    }
}
