using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RefactorName.Core
{
    /// <summary>
    /// Represents errors that occurs during retriving single business entity from Repository.
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Gets Business Entity name that dosn't found.
        /// </summary>
        public string EntityName { get; private set; }

        /// <summary>
        /// Gets the Business Entity Id that is requested from repository and dosn't found.
        /// </summary>
        public object EntityID { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        public EntityNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified business entity name, and its identity number.
        /// </summary>
        /// <param name="entityName">business enitity name that doesn't found.</param>
        /// <param name="entityId">business entity identity number that used to query from repository.</param>
        public EntityNotFoundException(string entityName, object entityId)
            : base(string.Format("{0} object with {1} not exists in repository.", entityName, entityId))
        {
            this.EntityName = entityName;
            this.EntityID = entityId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with specified business entity name, identity number and error message.
        /// </summary>
        /// <param name="entityName">business enitity name that doesn't found.</param>
        /// <param name="entityId">business entity identity number that used to query from repository.</param>
        /// <param name="message">custom message that describes the error.</param>
        public EntityNotFoundException(string entityName, object entityId, string message)
            : base(message)
        {
            this.EntityID = entityId;
            this.EntityName = entityName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with specified business entity name, identity number, error message, and a reference to the inner exception that is the cuase of this exception.
        /// </summary>
        /// <param name="entityName">business enitity name that doesn't found.</param>
        /// <param name="entityId">business entity identity number that used to query from repository.</param>
        /// <param name="message">custom message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public EntityNotFoundException(string entityName, object entityId, string message, Exception inner)
            : base(message, inner)
        {
            this.EntityName = entityName;
            this.EntityID = entityId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            EntityName = info.GetString("EntityName");
            EntityID = info.GetString("EntityID");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("EntityName", EntityName);
            info.AddValue("EntityID", EntityID);
        }
    }
}
