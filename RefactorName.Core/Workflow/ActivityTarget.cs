using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    public class ActivityTarget
    {
        public int ActivityTargetId { get; private set; }

        public int TargetId { get; private set; }


        //[Associated]
        public Target Target { get; private set; }

        public int ActivityId { get; private set; }

        //[Associated]
        public Activity Activity { get; private set; }

        public int GroupId { get; private set; }

        //[Associated]
        public Group Group { get; private set; }

        public ActivityTarget()
        {

        }
        public ActivityTarget(Activity activity, Target target, Group group)
        {
            this.Activity = activity;
            this.Target = target;
            this.Group = group;
        }
        public ActivityTarget Update(Activity activity, Target target, Group group)
        {
            this.ActivityId = activity.ActivityId;
            this.TargetId = target.TargetId;
            this.GroupId = group.GroupId;
            return this;
        }
    }
}
