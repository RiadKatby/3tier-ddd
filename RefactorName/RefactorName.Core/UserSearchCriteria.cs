
namespace RefactorName.Core
{
    public class UserSearchCriteria : SearchCriteria
    {
        public bool? IsActive { get; set; }
        
        public string UserName { get; set; }

        public  string FullName{ get; set; }

        public string RoleName { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

    }
}
