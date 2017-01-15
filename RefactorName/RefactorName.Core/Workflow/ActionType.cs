using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class ActionType
    {
        public int ActionTypeId { get; private set; }

        [Required, StringLength(100)]
        public string Name { get; private set; }
    }
}
