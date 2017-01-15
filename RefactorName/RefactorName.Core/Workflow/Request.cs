using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class Request
    {
        public int RequestId { get; private set; }

        public int UserRequestedId { get; private set; }

        public DateTime DateRequested { get; private set; }

        public User UserRequested { get; private set; }

        [Required]
        public IList<RequestData> Data { get; private set; }

        public IList<RequestNote> Notes { get; private set; }

        public IList<User> Stakeholders { get; private set; }

        public IList<RequestAction> History { get; private set; }
    }
}
