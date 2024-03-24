using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Sistem_za_upravljanje_sadrzajima
{
    public class XMLRead
    {   
        public List<User> readUser(string path)
        {
            try
            {
                List<User> users = new List<User>();
                var serializer = new XmlSerializer(users.GetType());

                using (var reader = new StreamReader(path))
                {
                    users = (List<User>)serializer.Deserialize(reader);
                }
                return users;
            }
            catch(Exception)
            {
                MessageBox.Show("Greska prilikom citanja usera.", "Greska!!!",MessageBoxButton.OK,MessageBoxImage.Error);
                return null;
            }
        }
    }
}
