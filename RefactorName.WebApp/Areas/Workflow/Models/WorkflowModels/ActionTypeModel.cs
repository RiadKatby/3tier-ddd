using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.WebApp.Areas.Workflow.Models
{
    /// <summary>
    /// Group of actions.
    /// It is possible that a request will need multiple <see cref="Actions"/> to happen before it can continue in the process
    /// </summary>
    public class ActionTypeModel
    {
        /// <summary>
        /// Gets identity number of this <see cref="ActionTypeModel"/> object.
        /// </summary>
        public int ActionTypeId { get;  set; }

        /// <summary>
        /// Gets name of this <see cref="ActionTypeModel"/> object.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get;  set; }

        /// <summary>
        /// Instanciate empty <see cref="ActionTypeModel"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public ActionTypeModel() { }

    }
}
