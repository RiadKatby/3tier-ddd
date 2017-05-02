using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.SearchEntities
{
    public class TransitionSearchCriteria: SearchCriteria
    {
        public int? ProcessId { get; set; }

        public int? CurrentStateId { get; set; }

        public int? NextStateId { get; set; }
    }
}
