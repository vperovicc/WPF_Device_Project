using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sistem_za_upravljanje_sadrzajima
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {

        private Device device;
        public InfoWindow()
        {
            InitializeComponent();
        }

        public InfoWindow(Device device)
        {
            InitializeComponent();
            this.device = device;

            lbName.Content = device.Name;
            lbNumber.Content = device.Br;
            lbDateAdded.Content = device.DateAdded;

            string absolutePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, device.PicturePath);
            imgPicture.Source = new BitmapImage(new Uri(absolutePath));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
