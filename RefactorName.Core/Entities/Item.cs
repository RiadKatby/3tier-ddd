using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Entities
{
    [DisplayName("المادة")]
    public class Item
    {
        public int ItemId { get; private set; }

        [Required, StringLength(50)]
        public string Name { get; private set; }

        public Item() { }

        public Item(string name)
        {
            this.Name = name;
        }

        public Item Update(string name)
        {
            this.Name = name;

            return this;
        }
    }
}
