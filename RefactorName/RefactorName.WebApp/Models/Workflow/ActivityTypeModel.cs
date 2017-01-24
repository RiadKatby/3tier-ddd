using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.WebApp.Models
{
    /// <summary>
    /// Group of <see cref="ActivityModel"/>.
    /// </summary>
    public class ActivityTypeModel
    {
        /// <summary>
        /// Gets identity number of this <see cref="ActivityTypeModel"/> object.
        /// </summary>
        public int ActivityTypeId { get;  set; }

        /// <summary>
        /// Gets <see cref="ActivityType"/> name.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get;  set; }

        /// <summary>
        /// Instanciate empty <see cref="ActivityTypeModel"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public ActivityTypeModel() { }

    }
}
