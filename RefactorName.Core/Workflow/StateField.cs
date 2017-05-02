using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// Represent information of <see cref="Field"/> when a specific <see cref="State"/> is entered.
    /// </summary>
    public class StateField
    {
        /// <summary>
        /// Gets identity number of <see cref="StateField"/> object.
        /// </summary>
        public int StateFieldId { get; private set; }

        /// <summary>
        /// Gets identity number of the <see cref="State"/> which has this <see cref="StateField"/>.
        /// </summary>
        public int StateId { get; private set; }

        /// <summary>
        /// Gets identity number of the <see cref="Field"/> object.
        /// </summary>
        public int FieldId { get; private set; }

        /// <summary>
        /// Gets flag that determin that <see cref="Field"/> is editable in <see cref="State"/>.
        /// </summary>
        public bool IsEditable { get; private set; }

        /// <summary>
        /// Gets flag that determin that <see cref="Field"/> is required in <see cref="State"/>.
        /// </summary>
        public bool IsRequired { get; private set; }

        /// <summary>
        /// Gets <see cref="Field"/> object.
        /// </summary>
        [Associated]
        public Field Field { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="StateField"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public StateField() { }

        /// <summary>
        /// Instanciate custom <see cref="StateField"/> object.
        /// </summary>
        /// <param name="field"><see cref="Field"/> object.</param>
        /// <param name="isRequired">flag determine that <see cref="Field"/> is required.</param>
        /// <param name="isEditable">flag determine that <see cref="Field"/> is editable.</param>
        public StateField(Field field, bool isRequired, bool isEditable)
        {
            this.Field = field;
            this.IsRequired = isRequired;
            this.IsEditable = isEditable;
        }
    }
}
