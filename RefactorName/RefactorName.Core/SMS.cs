using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    public class SMS
    {
        public string From { get; private set; }

        public string To { get; private set; }

        public string Body { get; private set; }

        public SMS()
        {

        }

        public SMS(string from, string to, string body) : this()
        {
            this.From = from;
            this.To = to;
            this.Body = body;
        }

    }
}
