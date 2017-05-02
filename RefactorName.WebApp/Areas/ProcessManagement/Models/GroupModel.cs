using RefactorName.Core;
using RefactorName.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    [Serializable]
    public class GroupModel
    {
        [Display(Name = "Process Name")]
        public int ProcessId { get; set; }

        [Display(Name = "Group Name")]
        [Required(ErrorMessage = "Please Enter")]
        [StringLength(100, ErrorMessage = "It should be no more than 100 characters")]
        public string Name { get; set; }

        [Display(Name = "Process Name")]
        public string ProcessName { get; set; }

        [Display(Name = "User Name")]
        public int UserId { get; set; }

        [Display(Name = "Group Name")]
        public int GroupId { get; set; }

        public Dictionary<string, string> UserNames { get; set; }

        [Display(Name = "Users")]
        public string[] Users { get; set; }

        public List<UserModel> Memebers { get; set; }

        public GroupModel FillDDLsWithUsers()
        {
            UserNames = UserService.Obj.GetAllUsersDictionary();
            return this;
        }
    }
    [Serializable]
    public class GroupAddModel : GroupModel
    {
        public GroupAddModel()
        {
            Memebers = new List<UserModel>();
        }
        public new GroupAddModel FillDDLsWithUsers()
        {
            UserNames = UserService.Obj.GetAllUsersDictionary();
            return this;
        }
    }
}