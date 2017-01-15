using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class State
    {
        public int StateId { get; private set; }

        public int ProcessId { get; private set; }

        public int StateTypeId { get; private set; }

        [Required, StringLength(100)]
        public string Name { get; private set; }

        [StringLength(255)]
        public string Description { get; private set; }

        public StateType Type { get; private set; }

        public IList<Activity> Activities { get; private set; }
    }
}
