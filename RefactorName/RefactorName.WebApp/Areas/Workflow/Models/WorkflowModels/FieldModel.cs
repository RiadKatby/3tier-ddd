using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.WebApp.Areas.Workflow.Models
{
    /// <summary>
    /// Fields that may filled within the <see cref="Request"/> through out <see cref="Process"/>.
    /// </summary>
    public class FieldModel
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
        [Required]
        public string Name { get; private set; }

        /// <summary>
        /// Gets <see cref="FieldType"/> of this <see cref="Field"/> object.
        /// </summary>
        //public FieldTypeModel Type { get; private set; }
        [Required]
        public int Type { get; private set; }

        /// <summary>
        /// Gets length of this <see cref="Field"/> object.
        /// </summary>
        [Required]
        public int Length { get; private set; }

        public FieldModel() { }

    }
}
