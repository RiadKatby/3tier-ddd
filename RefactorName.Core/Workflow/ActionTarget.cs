using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    public class ActionTarget
    {
        public int ActionTargetId { get; private set; }

        public int TargetId { get; private set; }


        public Target Target { get; private set; }

        public int ActionId { get; private set; }

        public Action Action { get; private set; }

        public int GroupId { get; private set; }

        public Group Group { get; private set; }

        public ActionTarget()
        {

        }
        public ActionTarget(Action action,Target target,Group group)
        {
            this.Action = action;
            this.Target = target;
            this.Group = group;
        }
        public ActionTarget Update(Action action, Target target, Group group)
        {
            this.ActionId = action.ActionId;
            this.TargetId = target.TargetId;
            this.GroupId = group.GroupId;
            return this;
        }
    }
}
