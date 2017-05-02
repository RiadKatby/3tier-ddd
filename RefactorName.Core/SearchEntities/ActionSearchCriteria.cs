using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.SearchEntities
{
    public class ActionSearchCriteria: SearchCriteria
    {
        public int? ActionTypeId { get; set; }

        public int? ProcessId { get; set; }

        public int? TransitionId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
