using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for AddPlayer.xaml
    /// </summary>
    public partial class AddPlayer : Window
    {
        //Declare class vars.
        public byte[] imagearray;
        public bool canceled;
        public bool hasimage;


        public AddPlayer()
        {
            InitializeComponent();
        }
        

        private void button_Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog filepicker = new OpenFileDialog();
            filepicker.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            filepicker.ShowDialog();
            imagearray = File.ReadAllBytes(filepicker.FileName);
            MemoryStream ms = new MemoryStream(imagearray);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            playerImage.Source = bitmap;
            hasimage = true;


        }

        private void button_Clear_Click(object sender, RoutedEventArgs e)
        {
            playerImage.Source = null;
        }

        private void textBox_Player_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(textBox_Character.Text == "") && !(textBox_Player.Text == ""))
            { 
                button_OK.IsEnabled = true;
            }
        }

        private void textBox_Player_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!(textBox_Character.Text == "") && !(textBox_Player.Text == ""))
            {
                button_OK.IsEnabled = true;
            }
        }

        private void textBox_Character_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!(textBox_Character.Text == "") && !(textBox_Player.Text == ""))
            {
                button_OK.IsEnabled = true;
            }
        }

        private void textBox_Character_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(textBox_Character.Text == "") && !(textBox_Player.Text == ""))
            {
                button_OK.IsEnabled = true;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            canceled = true;
            Close();
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            canceled = false;
            Close();
        }
    }
}
