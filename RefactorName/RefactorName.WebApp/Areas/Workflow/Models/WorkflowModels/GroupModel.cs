using RefactorName.WebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.WebApp.Areas.Workflow.Models
{
    /// <summary>
    /// Group is a collection of Users that perform related functions.
    /// </summary>
    public class GroupModel
    {
        /// <summary>
        /// Gets identity number of <see cref="Group"/> object.
        /// </summary>
        public int GroupId { get;  set; }

        /// <summary>
        /// Gets identity number of the <see cref="Process"/> which has this <see cref="Group"/>.
        /// </summary>
        public int ProcessId { get;  set; }

        /// <summary>
        /// Gets the name of this <see cref="Group"/>.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get;  set; }

        /// <summary>
        /// Gets list of <see cref="User"/>s that are Members of this <see cref="Group"/> object.
        /// </summary>
        [Required]
        public IList<UserModel> Members { get; set; }

        /// <summary>
        /// Instanciate empty <see cref="Group"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public GroupModel()
        {
            this.Members = new List<UserModel>();
        }

    
    }
}
