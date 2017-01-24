using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.WebApp.Models
{
    /// <summary>
    /// Type of data Field.
    /// </summary>
    public class FieldTypeModel
    {
        /// <summary>
        /// Gets identity number of <see cref="FieldTypeModel"/> object.
        /// </summary>
        public int FieldTypeId { get; set; }

        /// <summary>
        /// Gets the name of this <see cref="FieldTypeModel"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the full type name.
        /// </summary>
        public string TypeFullName { get; set; }

        /// <summary>
        /// Instanciate empty <see cref="FieldTypeModel"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public FieldTypeModel() { }
        
    }
}
