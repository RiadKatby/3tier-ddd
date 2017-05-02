using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    public class ActiveDirectoryUserInfo
    {
        public string Department { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CreatedDate { get; set; }
        public string IpPhone { get; set; }
        public byte[] ThumbnailPhoto { get; set; }
    }
}
