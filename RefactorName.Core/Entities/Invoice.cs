using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; private set; }

        public DateTime CreateDate { get; private set; }

        [Owned]
        public List<InvoiceDetail> Details { get; private set; }

        public Invoice()
        {
            this.CreateDate = DateTime.Now;
            this.Details = new List<InvoiceDetail>();
        }
    }
}
