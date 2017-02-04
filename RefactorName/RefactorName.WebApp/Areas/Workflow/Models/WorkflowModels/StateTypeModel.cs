using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.WebApp.Areas.Workflow.Models
{
    /// <summary>
    /// group of <see cref="State"/>s.
    /// </summary>
    public class StateTypeModel
    {
        /// <summary>
        /// Gets identity number of <see cref="StateTypeModel"/> object.
        /// </summary>
        public int StateTypeId { get; private set; }

        /// <summary>
        /// Gets the name of this <see cref="StateTypeModel"/>.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="StateTypeModel"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public StateTypeModel() { }
    }
}
