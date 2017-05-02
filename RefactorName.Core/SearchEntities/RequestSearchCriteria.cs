using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.SearchEntities
{
    public class RequestSearchCriteria : SearchCriteria
    {
        public string Title { get; set; }

        public int? UserRequestedId { get; set; }

        public int? CurrentStateId { get; set; }

        public DateTime? DateRequested { get; set; }

        public User UserRequested { get; set; }

        public State CurrentState { get; set; }

    }
}
