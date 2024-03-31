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
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;

namespace Sistem_za_upravljanje_sadrzajima
{
    /// <summary>
    /// Interaction logic for useWindow.xaml
    /// </summary>
    public partial class UseWindow : Window
    {

        public string name;

        public ObservableCollection<Device> devices = new ObservableCollection<Device>();

        List<User> users = new List<User>();
        User userPom = new User();

        //-----------------------LOADING THE WINDOW---------------------------
        public UseWindow()
        {
            InitializeComponent();
        }

        //-----------------------LOADING THE WINDOW AND LOADING THE DEVICES AND USERS FROM XML FILES---------------------------
        public UseWindow(User user)
        {
            InitializeComponent();
            if(user.Role==UserRole.Visitor)
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
            }

            XMLDeviceRead devReader = new XMLDeviceRead();
            devices = new ObservableCollection<Device>(devReader.readDevice(@"../../XML_file/device_info.xml"));
            dgContent.DataContext = devices;

            XMLRead reader = new XMLRead();
            users = reader.readUser(@"../../XML_file/user_info.xml");

            if(user.Role==UserRole.Admin)
            {
                userPom = users[0];
            }
            else if(user.Role==UserRole.Visitor)
            {
                userPom = users[1];
            }
        }
        //-----------------------ON EXIT BUTTON CLICK SAVES THE CHANGES TO THE XML FILE AND CLOSES THE WINDOW---------------------------
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow passMain = new MainWindow(devices);
            passMain.Show();

            try
            {
                var serializer = new XmlSerializer(devices.GetType());
                using (var writer = new StreamWriter(@"../../XML_file/device_info.xml"))
                {
                    serializer.Serialize(writer, devices);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error while inserting into XML file.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }

        private void LoginPath_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //-----------------------CHECKING IF THE CHECKBOX IN THE DATAGRID IS CHECKED FOR DELETING---------------------------
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox check = sender as CheckBox;

            if(check!=null)
            {
                var bindingObject = (Device)check.DataContext;
                bindingObject.IsChecked = true;
            }
        }

        //-----------------------DELETING THE DEVICE FROM THE DATAGRID, XML FILE, AND ALSO REMOVING THE .RTF FILE---------------------------
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (checkDelete()==true)
            {

                var itemsToRemove = devices.Where(item => item.IsChecked).ToList();
                foreach (var item in itemsToRemove)
                {
                    devices.Remove(item);
                    if (File.Exists(item.RtfPath))
                    {
                        File.Delete(item.RtfPath);
                    }
                }
            }

            //foreach (var item in devices.ToList())
            //{
            //    if (item.IsChecked == true)
            //    {
            //        devices.Remove(item);
                    
            //    }
            //}
        }

        //-----------------------OPENS THE ADD/CHANGE WINDOW---------------------------
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ChangeWindow changeWindow = new ChangeWindow(this,devices);
            changeWindow.Show();
            return;
        }

        //-----------------------HYPERLINK CLICK BASED ON WHO IS LOGGED IN(ADMIN OR USER)---------------------------
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (userPom.Role == UserRole.Admin)
            {
                Hyperlink link = sender as Hyperlink;
                var device = (Device)link.DataContext;
                ChangeWindow changeWindow = new ChangeWindow(device, true, this);
                changeWindow.Show();
            }
            else if (userPom.Role == UserRole.Visitor)
            {
                Hyperlink link = sender as Hyperlink;
                var device = (Device)link.DataContext;
                InfoWindow infoWindow = new InfoWindow(device);
                infoWindow.Show();
            }
        }

        //-----------------------REFRESHES DATAGRID WHEN CHANGES ARE MADE---------------------------
        public void RefreshDataGrid()
        {
            dgContent.DataContext = null; 
            dgContent.DataContext = devices;
        }

        //-----------------------DO YOU REALLY WANT TO DELETE IT------------------------------------

        public bool checkDelete()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the device?","Information!",MessageBoxButton.YesNo,MessageBoxImage.Information);

            if(result==MessageBoxResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
