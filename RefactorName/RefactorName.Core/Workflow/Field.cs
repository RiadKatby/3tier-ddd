using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class Field
    {
        public int FieldId { get; private set; }
        public int ProcessId { get; private set; }
        public string Name { get; private set; }
        public int Type { get; private set; }
        public int Length { get; private set; }
    }
}
