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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

namespace Sistem_za_upravljanje_sadrzajima
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<User> users = new List<User>();
        ObservableCollection<Device> passedDevices;
        UseWindow useWindow = new UseWindow();

        //-----------------------LOADING THE WINDOWS---------------------------
        public MainWindow()
        {
            InitializeComponent();
            XMLRead reader = new XMLRead();
            users = reader.readUser(@"../../XML_file/user_info.xml");
            tbUsername.Focus();
        }

        public MainWindow(ObservableCollection<Device> devs)
        {
            InitializeComponent();
            XMLRead reader = new XMLRead();
            users = reader.readUser(@"../../XML_file/user_info.xml");
            passedDevices = devs;
            tbUsername.Focus();
        }


        //-----------------------EXIT BUTTON(CLOSE THE APP)---------------------------
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            return;
        }

        //-----------------------LOGIN ACTION---------------------------
        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbUsername.Text == String.Empty)
                {
                    MessageBox.Show("Username field empty.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    pbPassword.Clear();
                    tbUsername.Clear();
                    return;
                }

                if (pbPassword.Password == String.Empty)
                {
                    MessageBox.Show("Password field empty.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    pbPassword.Clear();
                    tbUsername.Clear();
                    return;
                }

                if (tbUsername.Text == users[0].Username)
                {
                    if (pbPassword.Password != users[0].Password)
                    {
                        MessageBox.Show("Password incorrect.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        pbPassword.Clear();
                        tbUsername.Clear();
                    }
                    else
                    {
                        UseWindow newWindow = new UseWindow(users[0]);
                        newWindow.ShowDialog();
                        Close();
                    }
                }
                else if (tbUsername.Text == users[1].Username)
                {
                    if (pbPassword.Password != users[1].Password)
                    {
                        MessageBox.Show("Password incorrect.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        pbPassword.Clear();
                        tbUsername.Clear();
                    }
                    else
                    {
                        UseWindow newWindow = new UseWindow(users[1]);
                        newWindow.ShowDialog();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Username not found.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    pbPassword.Clear();
                    tbUsername.Clear();
                }

                tbUsername.Clear();
                pbPassword.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoginPath_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //-----------------------ENABLES SO WHEN I PRESS ENTER ON KEYBOARD THE LOGIN BUTTON IS INITIATED---------------------------
        private void tbUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogIn_Click(sender, new RoutedEventArgs());
            }
        }

        private void pbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogIn_Click(sender, new RoutedEventArgs());
            }
        }
    }

}
