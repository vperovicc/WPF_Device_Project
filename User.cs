using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistem_za_upravljanje_sadrzajima
{
    public enum UserRole { Visitor, Admin }

    [Serializable]
    public class User
    {
        private string username;
        private string password;
        private UserRole role;

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public UserRole Role { get => role; set => role = value; }

        public User()
        { }

        public User(string un, string pw, UserRole rl)
        {
            this.username = un;
            this.password = pw;
            this.role = rl;
        }


    }
}
