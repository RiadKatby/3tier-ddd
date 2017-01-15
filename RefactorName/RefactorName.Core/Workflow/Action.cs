using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class Action
    {
        public int ActionId { get; private set; }

        public int ProcessId { get; private set; }

        public int ActionTypeId { get; private set; }

        [Required, StringLength(100)]
        public string Name { get; private set;}

        [StringLength(255)]
        public string Description { get; private set; }

        public ActionType ActionType { get; private set; }

        public Action() { }

        public Action(string name, string description, ActionType actionType)
        {
            this.Name = name;
            this.Description = description;

            this.ActionType = actionType;
            this.ActionTypeId = actionType.ActionTypeId;
        }

        public Action Update(string name, string description, ActionType actionType)
        {
            this.Name = name;
            this.Description = description;

            this.ActionType = actionType;
            this.ActionTypeId = actionType.ActionTypeId;

            return this;
        }
    }
}
