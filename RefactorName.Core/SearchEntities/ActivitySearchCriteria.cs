using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.SearchEntities
{
    public class ActivitySearchCriteria: SearchCriteria
    {
        public int? ActivityTypeId { get; set; }

        public int? ProcessId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
