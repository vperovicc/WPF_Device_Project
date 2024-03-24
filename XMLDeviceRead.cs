using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Windows;

namespace Sistem_za_upravljanje_sadrzajima
{
    public class XMLDeviceRead
    {
        public List<Device> readDevice(string path)
        {
            try
            {
                List<Device> devices = new List<Device>();
                var serializer = new XmlSerializer(devices.GetType());

                using (var reader = new StreamReader(path))
                {
                    devices = (List<Device>)serializer.Deserialize(reader);
                }
                return devices;
            }
            catch (Exception)
            {
                MessageBox.Show("Reading error.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
