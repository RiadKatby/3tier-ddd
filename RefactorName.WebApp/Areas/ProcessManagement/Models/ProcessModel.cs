using RefactorName.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static RefactorName.Core.Enum;

namespace RefactorName.WebApp
{
    [Serializable]
    public class ProcessModel
    {
        public int ProcessId { get; set; }

        [Display(Name = "Process Name")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [StringLength(100, ErrorMessage = "It should be no more than 100 characters")]
        public string Name { get; set; }

        public Dictionary<string, string> UserNames { get; set; }

        [Display(Name = "Users")]
        public string[] Users { get; set; }

        public ProcessModel FillDDLsWithUsers()
        {
            UserNames = UserService.Obj.GetAllUsersDictionary();
            return this;
        }

        public List<UserModel> Admins { get; set; }

        public Status ProcessStatus { get; set; }

        //public List<UserAddModel>
        //[Display(Name = "User")]
        //[Required(ErrorMessage = "Please Enter {0}")]
        //public int? User { get; set; }

        //[Display(Name = "User Name")]

        //public string UserName { get; set; }


    }    
    public class ProcessAddModel : ProcessModel
    {
        public ProcessAddModel()
        {
            this.Admins = new List<UserModel>();
        }
        public new ProcessAddModel FillDDLsWithUsers()
        {
            UserNames = UserService.Obj.GetAllUsersDictionary();
            return this;
        }
    }    
}