using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class StateField
    {
        public int StateFieldId { get; private set; }

        public int StateId { get; private set; }

        public int FieldId { get; private set; }

        public bool IsEditable { get; private set; }

        public bool IsRequired { get; private set; }

        public State State { get; private set; }

        public Field Field { get; private set; }
    }
}
