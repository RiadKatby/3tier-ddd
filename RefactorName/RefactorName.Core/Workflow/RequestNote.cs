using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class RequestNote
    {
        public int RequestNoteId { get; private set; }

        public int RequestId { get; private set; }

        public int StateId { get; private set; }

        public int UserId { get; private set; }

        public string Note { get; private set; }

        public State State { get; private set; }

        public User User { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="RequestNote"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public RequestNote()
        {

        }
    }
}
