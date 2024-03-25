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
using System.Text.RegularExpressions;
using System.IO;

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
        string passedDeviceRtfPath;

        private Device passedDevice;

        public void loadToolbar()
        {
            FontComboBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);

            List<double> fontSizes = new List<double>();
            for (double i = 8; i <= 72; i+=2)
            {
                fontSizes.Add(i);
            }
            cbFontSize.ItemsSource = fontSizes;
        }

        public ChangeWindow(UseWindow useWindow)
        {
            InitializeComponent();
            this.useWindow = useWindow;
            loadToolbar();
        }
        
        public ChangeWindow()
        {
            InitializeComponent();
            loadToolbar();
        }

        public ChangeWindow(Device device, bool isEditing, UseWindow useWind)
        {
            InitializeComponent();
            loadToolbar();
            useWindPom = useWind;

            passedDevice = device;

            if(isEditing)
            {
                tbNumber.Text = device.Br.ToString();
                tbName.Text = device.Name;
                picPath = device.PicturePath;
                string absolutePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, device.PicturePath);
                imgPicture.Source = new BitmapImage(new Uri(absolutePath));
                if (File.Exists(device.RtfPath))
                {
                    TextRange textRange = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd);

                    using (FileStream fs = new FileStream(device.RtfPath, FileMode.Open))
                    {
                        textRange.Load(fs, DataFormats.Rtf);
                    }
                }
                else
                {
                    MessageBox.Show("The RTF file does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                        passedDeviceRtfPath = passedDevice.RtfPath;
                        passedDevice.RtfPath =  updateRtfContent(passedDeviceRtfPath);
                        useWindPom.RefreshDataGrid();
                        MessageBox.Show("Success!", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else
                    {
                        newDevice.Br = Convert.ToInt32(tbNumber.Text);
                        newDevice.Name = tbName.Text;
                        newDevice.DateAdded = DateTime.Now;
                        newDevice.PicturePath = picPath;
                        string nameOfRtf = tbName.Text + "RTF";
                        rtfCreation(nameOfRtf, rtbText);
                        newDevice.RtfPath = "../../rtfs/"+ nameOfRtf+".rtf";
                        useWindow.devices.Add(newDevice);
                        MessageBox.Show("Success!", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
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

            TextRange textRange = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd);

            if(textRange.Text=="\r\n")
            {
                MessageBox.Show("No input for text.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
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

        private void FontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontComboBox.SelectedItem != null && !rtbText.Selection.IsEmpty)
            {
                rtbText.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontComboBox.SelectedItem);
            }
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFontSize.SelectedItem != null && !rtbText.Selection.IsEmpty)
            {
                rtbText.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cbFontSize.SelectedItem);
            }
        }

        private void cpColor_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cpColor.SelectedColor.HasValue)
            {
                Color selectedColor = cpColor.SelectedColor.Value;

                SolidColorBrush newBrush = new SolidColorBrush(selectedColor);

                rtbText.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, newBrush);
            }
        }

        private void updateWordCounter(string text)
        {
            string pattern = @"\b\w+\b";
            MatchCollection matches = Regex.Matches(text, pattern);

            tbWordCounter.Text = "Word Count: " + matches.Count;
        }
        private void rtbText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd).Text;
            updateWordCounter(text);
        }

        public void rtfCreation(string name, RichTextBox rtb)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);

                if (!textRange.IsEmpty)
                {
                    textRange.Save(stream, DataFormats.Rtf);
                    stream.Position = 0;

                    StreamReader reader = new StreamReader(stream);
                    string rtfContent = reader.ReadToEnd();
                    reader.Close();
                    stream.Close();
                    string directoryPath = "../../rtfs/";

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string filePath = System.IO.Path.Combine(directoryPath, name + ".rtf");
                    File.WriteAllText(filePath, rtfContent);
                }
                else
                {
                    MessageBox.Show("The RichTextBox is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the RTF content: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string updateRtfContent(string rtfFilePath)
        {
            if (File.Exists(rtfFilePath))
            {
                using (FileStream fs = new FileStream(rtfFilePath, FileMode.Create))
                {
                    TextRange range = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd);
                    range.Save(fs, DataFormats.Rtf);
                }
            }
            return rtfFilePath;
        }

        private void rtbText_SelectionChanged(object sender, RoutedEventArgs e)
        {

            object fontWeight = rtbText.Selection.GetPropertyValue(Inline.FontWeightProperty);
            object fontStyle = rtbText.Selection.GetPropertyValue(Inline.FontStyleProperty);
            object textDecoration = rtbText.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            object fontFamily = rtbText.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            object fontSize = rtbText.Selection.GetPropertyValue(Inline.FontSizeProperty);
            object textColor = rtbText.Selection.GetPropertyValue(Inline.ForegroundProperty);

            BoldToggleButton.IsChecked = (fontWeight != DependencyProperty.UnsetValue) && (fontWeight.Equals(FontWeights.Bold));
            ItalicToggleButton.IsChecked = (fontStyle != DependencyProperty.UnsetValue) && (fontStyle.Equals(FontStyles.Italic));
            UnderlineToggleButton.IsChecked = (textDecoration != DependencyProperty.UnsetValue) && (textDecoration.Equals(TextDecorations.Underline));
            FontComboBox.SelectedItem = fontFamily;
            cbFontSize.SelectedItem = fontSize;
            if (textColor is SolidColorBrush)
            {
                SolidColorBrush brush = (SolidColorBrush)textColor;
                Color color = brush.Color;
                cpColor.SelectedColor = color;
            }
        }
    }
}
