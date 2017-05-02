using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.SearchEntities
{
    public class GroupSearchCriteria: SearchCriteria
    {
        public int? ProcessId { get; set; }

        public string Name { get; set; }
    }
}
