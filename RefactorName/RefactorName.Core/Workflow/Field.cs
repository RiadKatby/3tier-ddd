using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    /// <summary>
    /// Fields that may filled within the <see cref="Request"/> through out <see cref="Process"/>.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Gets identity number of <see cref="Field"/> object.
        /// /// </summary>
        public int FieldId { get; private set; }

        /// <summary>
        /// Gets identity number of the <see cref="Process"/> which has this <see cref="Field"/>.
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// Gets the name of this <see cref="Field"/>.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets <see cref="FieldType"/> of this <see cref="Field"/> object.
        /// </summary>
        [Associated]
        public FieldType Type { get; private set; }

        /// <summary>
        /// Gets length of this <see cref="Field"/> object.
        /// </summary>
        public int Length { get; private set; }

        public Field() { }

        public Field(string name, FieldType fieldType, int length)
        {
            this.Name = name;
            this.Type = fieldType;
            this.Length = length;
        }

    }
}
