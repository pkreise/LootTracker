using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Media;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for AddPlayer.xaml
    /// </summary>
    public partial class AddPlayer : Window
    {
        //Class fields.
        byte[] _imagearray;
        bool _cancelled;
        bool _hasimage;

        //public properties.
        public byte[] imagearray { get { return _imagearray; } }
        public bool cancelled { get { return _cancelled; } }
        public bool hasimage { get { return _hasimage; } }

        //AddPlayer window entry point.
        public AddPlayer()
        {
            InitializeComponent();
        }
        
        //Event handler for clicking the browse button.
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

        //Event handler for clicking the clear photo button.
        private void button_Clear_Click(object sender, RoutedEventArgs e)
        {
            playerImage.Source = null;
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            //Working here!!!!
            bool namevalid = false;
            bool charactervalid = false;
            
            //Field validation for Name.
            if (textBox_Player.Text == "")
            {
                textBlock_Player.Foreground = Brushes.Red;
                textBlock_Player.ToolTip = "Please enter a Player name.";
            }
            else
            {
                textBlock_Player.Foreground = Brushes.Black;
                namevalid = true;
                textBlock_Player.ToolTip = null;
            }

            //Field Validation for Character.
            if (textBox_Character.Text == "")
            {
                textBlock_Character.Foreground = Brushes.Red;
                textBlock_Character.ToolTip = "Please enter a Character name.";
            }
            else
            {
                textBlock_Character.Foreground = Brushes.Black;
                charactervalid = true;
                textBlock_Character.ToolTip = null;
            }

            //If both fieds are valid, close.
            if (namevalid && charactervalid)
            {
                _cancelled = false;
                Close();
            }
        }

        //Event handler for close button.
        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            _cancelled = true;
            Close();
        }

        //Event handler for window drag.
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            DragMove();
        }
    }
}
