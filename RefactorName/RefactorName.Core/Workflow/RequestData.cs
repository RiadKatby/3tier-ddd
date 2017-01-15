using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    public class RequestData
    {
        public int RequestDataId { get; private set; }

        public int RequestId { get; private set; }

        public int FieldId { get; private set; }

        public string Value { get; private set; }

        public int ValueIndex { get; private set; }

        public Field Field { get; private set; }
    }
}
