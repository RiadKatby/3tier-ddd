using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// Type of data Field.
    /// </summary>
    public class FieldType
    {
        /// <summary>
        /// Integer <see cref="FieldType"/>.
        /// </summary>
        public static readonly FieldType IntType = new FieldType { FieldTypeId = 1, Name = "Integer", TypeFullName = typeof(int).FullName };

        /// <summary>
        /// String <see cref="FieldType"/>.
        /// </summary>
        public static readonly FieldType StringType = new FieldType { FieldTypeId = 2, Name = "String", TypeFullName = typeof(string).FullName };

        /// <summary>
        /// Gets identity number of <see cref="FieldType"/> object.
        /// </summary>
        public int FieldTypeId { get; private set; }

        /// <summary>
        /// Gets the name of this <see cref="FieldType"/>.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the full type name.
        /// </summary>
        public string TypeFullName { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="FieldType"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public FieldType() { }

        /// <summary>
        /// Instanciate custom <see cref="FieldType"/> object.
        /// </summary>
        /// <param name="name">name of <see cref="FieldType"/></param>
        /// <param name="typeFullName">full type name of <see cref="FieldType"/></param>
        public FieldType(string name, string typeFullName)
        {
            this.Name = name;
            this.TypeFullName = typeFullName;
        }
    }
}
