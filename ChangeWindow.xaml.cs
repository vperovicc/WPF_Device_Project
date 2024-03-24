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
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace Sistem_za_upravljanje_sadrzajima
{
    /// <summary>
    /// Interaction logic for ChangeWindow.xaml
    /// </summary>
    public partial class ChangeWindow : Window
    {
        Device newDevice = new Device();
        private UseWindow useWindow;
        private UseWindow useWindPom;
        string picPath;

        private Device passedDevice;

        public ChangeWindow(UseWindow useWindow)
        {
            InitializeComponent();
            this.useWindow = useWindow;
            List<FontFamily> fontFamilies = Fonts.SystemFontFamilies.ToList();

            // Populate the ComboBox with font family names
            foreach (var fontFamily in fontFamilies)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = fontFamily.ToString();
                item.FontFamily = fontFamily;
                FontComboBox.Items.Add(item);
            }

            var fontSizes = new ObservableCollection<double>();
            for (double fontSize = 8; fontSize <= 72; fontSize += 2)
            {
                fontSizes.Add(fontSize);
            }

            // Set the font sizes collection as the ComboBox's ItemsSource
            cbFontSize.ItemsSource = fontSizes;

        }
        
        public ChangeWindow()
        {
            InitializeComponent();
        }

        public ChangeWindow(Device device, bool isEditing, UseWindow useWind)
        {
            InitializeComponent();

            useWindPom = useWind;

            passedDevice = device;

            if(isEditing)
            {
                tbNumber.Text = device.Br.ToString();
                tbName.Text = device.Name;
                picPath = device.PicturePath;
                string absolutePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, device.PicturePath);
                imgPicture.Source = new BitmapImage(new Uri(absolutePath));
            }
            else
            {
                tbName.Clear();
                tbNumber.Clear();
                imgPicture.Source = null;
                rtbText.SelectAll();
                rtbText.Selection.Text = "";
            }
        }

        private void LoginPath_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            return;
        }

        private void btnPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif)|*.jpg; *.jpeg; *.png; *.gif|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFileName = openFileDialog.FileName;
                picPath = selectedFileName;
            }

            imgPicture.Source = new BitmapImage(new Uri(picPath));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (security() != false)
                {
                    if (passedDevice != null)
                    {
                        passedDevice.Br = Convert.ToInt32(tbNumber.Text);
                        passedDevice.Name = tbName.Text;
                        passedDevice.DateAdded = DateTime.Now;
                        passedDevice.PicturePath = picPath;
                        passedDevice.RtfPath = "";
                        useWindPom.RefreshDataGrid();
                        MessageBox.Show("Success!", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        newDevice.Br = Convert.ToInt32(tbNumber.Text);
                        newDevice.Name = tbName.Text;
                        newDevice.DateAdded = DateTime.Now;
                        newDevice.PicturePath = picPath;
                        newDevice.RtfPath = "";
                        useWindow.devices.Add(newDevice);
                        MessageBox.Show("Success!", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                picPath = "";
            }
            catch(Exception)
            {
                MessageBox.Show("Error while adding the device.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ResetFields();
                Close();
            }
        }

        private void ResetFields()
        {
            tbName.Clear();
            tbNumber.Clear();
            imgPicture.Source = null;
            rtbText.SelectAll();
            rtbText.Selection.Text = "";
        }

        private bool security()
        {

            if (tbNumber.Text == string.Empty)
            {
                MessageBox.Show("No input for number.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (tbName.Text== string.Empty)
            {
                MessageBox.Show("No input for name.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if(picPath=="")
            {
                MessageBox.Show("You did not pick the picture.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void FontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontComboBox.SelectedItem != null)
            {
                var selectedItem = (ComboBoxItem)FontComboBox.SelectedItem;
                FontFamily fontFamily = selectedItem.FontFamily;
                rtbText.FontFamily = fontFamily;
            }
        }

        private void BoldToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (BoldToggleButton.IsChecked == true)
            {
                rtbText.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                rtbText.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void ItalicToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (ItalicToggleButton.IsChecked == true)
            {
                rtbText.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                rtbText.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void UnderlineToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (UnderlineToggleButton.IsChecked == true)
            {
                rtbText.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                rtbText.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFontSize.SelectedItem != null)
            {
                double selectedFontSize = (double)cbFontSize.SelectedItem;

                if (rtbText != null)
                {
                    rtbText.FontSize = selectedFontSize;
                }
            }
        }
    }
}
