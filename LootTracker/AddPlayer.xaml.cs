using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for AddPlayer.xaml
    /// </summary>
    public partial class AddPlayer : Window
    {
        //Declare class fields.
        byte[] _imagearray;
        bool _cancelled;
        bool _hasimage;

        public byte[] imagearray { get { return _imagearray; } }
        public bool cancelled { get { return _cancelled; } }
        public bool hasimage { get { return _hasimage; } }


        public AddPlayer()
        {
            InitializeComponent();
        }
        

        private void button_Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog filepicker = new OpenFileDialog();
            filepicker.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            filepicker.ShowDialog();

            //The user could cancel the file prompt, in which case
            //we don't want to try to read a null file.

            if (!(filepicker.FileName == ""))
            {
                _imagearray = File.ReadAllBytes(filepicker.FileName);
                MemoryStream ms = new MemoryStream(_imagearray);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                playerImage.Source = bitmap;
                _hasimage = true;
            }
            


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
            else
            {
                button_OK.IsEnabled = false;
            }
        }

        private void textBox_Player_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!(textBox_Character.Text == "") && !(textBox_Player.Text == ""))
            {
                button_OK.IsEnabled = true;
            }
            else
            {
                button_OK.IsEnabled = false;
            }
        }

        private void textBox_Character_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!(textBox_Character.Text == "") && !(textBox_Player.Text == ""))
            {
                button_OK.IsEnabled = true;
            }
            else
            {
                button_OK.IsEnabled = false;
            }
        }

        private void textBox_Character_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(textBox_Character.Text == "") && !(textBox_Player.Text == ""))
            {
                button_OK.IsEnabled = true;
            }
            else
            {
                button_OK.IsEnabled = false;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            _cancelled = true;
            Close();
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            _cancelled = false;
            Close();
        }

        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            DragMove();
        }
    }
}
