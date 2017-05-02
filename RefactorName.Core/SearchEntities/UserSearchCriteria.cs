using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.SearchEntities
{
    public class UserSearchCriteria: SearchCriteria
    {
        public int? UserId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public string FullName { get; set; }
        public int? GroupId { get; set; }

        public Group Group { get; set; }
        
    }
}
