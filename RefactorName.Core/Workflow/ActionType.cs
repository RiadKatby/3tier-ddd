using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// Group of actions.
    /// It is possible that a request will need multiple <see cref="Actions"/> to happen before it can continue in the process
    /// </summary>
    public class ActionType
    {
        /// <summary>
        /// The actioner is suggesting that the request should move to the next state.
        /// </summary>
        public static readonly ActionType Approve = new ActionType { ActionTypeId = 1, Name = "Approve" };

        /// <summary>
        /// The actioner is suggesting that the request should move to the previous state.
        /// </summary>
        public static readonly ActionType Deny = new ActionType { ActionTypeId = 2, Name = "Deny" };

        /// <summary>
        /// The actioner is suggesting that the request should move to the Cancelled state in the process.
        /// </summary>
        public static readonly ActionType Cancel = new ActionType { ActionTypeId = 3, Name = "Cancel" };

        /// <summary>
        /// The actioner suggesting that the request be moved back to the Start state in the process.
        /// </summary>
        public static readonly ActionType Restart = new ActionType { ActionTypeId = 4, Name = "Restart" };

        /// <summary>
        /// The actioner is suggesting that the request be moved all the way to the Completed state.
        /// </summary>
        public static readonly ActionType Resolve = new ActionType { ActionTypeId = 5, Name = "Resolve" };

        /// <summary>
        /// Gets identity number of this <see cref="ActionType"/> object.
        /// </summary>
        public int ActionTypeId { get; private set; }

        /// <summary>
        /// Gets name of this <see cref="ActionType"/> object.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="ActionType"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public ActionType() { }

        /// <summary>
        /// Instanciate custom <see cref="ActionType"/> object.
        /// </summary>
        /// <param name="name">name of actionType.</param>
        public ActionType(string name)
        {
            this.Name = name;
        }
    }
}
