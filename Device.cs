using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Sistem_za_upravljanje_sadrzajima
{
    [Serializable]
    public class Device
    {
        private int br;
        private string name;
        private string picturePath;
        private string rtfPath;
        private DateTime dateAdded;
        [XmlIgnore]
        private bool isChecked;

        public Device() { }

        public Device(int br, string name, string picturePath, string rtfPath, DateTime dateAdded)
        {
            this.br = br;
            this.name = name;
            this.picturePath = picturePath;
            this.rtfPath = rtfPath;
            this.dateAdded = dateAdded;
            IsChecked = false;
        }

        public int Br { get => br; set => br = value; }
        public string Name { get => name; set => name = value; }
        public string PicturePath { get => picturePath; set => picturePath = value; }
        public string RtfPath { get => rtfPath; set => rtfPath = value; }
        public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }
        [XmlIgnore]
        public bool IsChecked { get => isChecked; set => isChecked = value; }
    }
}
