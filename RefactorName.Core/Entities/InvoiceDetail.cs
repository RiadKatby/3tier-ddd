using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Entities
{
    public class InvoiceDetail
    {
        public int InvoiceDetailId { get; private set; }

        public int InvoiceId { get; private set; }

        public int? ItemId { get; private set; }

        public Invoice Invoice { get; private set; }

        [Associated]
        public Item Item { get; private set; }

        public InvoiceDetail()
        {
        }

        public InvoiceDetail(Item item)
        {
            Item = item;
            ItemId = item.ItemId;
        }
    }
}
