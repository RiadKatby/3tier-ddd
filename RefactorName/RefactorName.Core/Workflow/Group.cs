using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class Group
    {
        public int GroupId { get; private set; }

        [Required, StringLength(100)]
        public string Name { get; private set; }

        [Required]
        public IList<User> Members { get; private set; }
    }
}
