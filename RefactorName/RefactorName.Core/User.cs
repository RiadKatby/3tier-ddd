using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    public class User : IUser<int>
    {
        #region IUser<int> interface members

        public int Id
        {
            get { return UserId; }
            private set { UserId = value; }
        }

        public string UserName
        {
            get { return Username; }
            set { Username = value; }
        }

        #endregion

        public int UserId { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public object PasswordHash { get; set; }
        public object PhoneNumber { get; set; }

        public User()
        {

        }
    }
}
