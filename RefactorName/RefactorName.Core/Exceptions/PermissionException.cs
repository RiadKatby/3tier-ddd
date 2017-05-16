using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RefactorName.Core
{
    /// <summary>
    /// Represents errors that occurs during security permissions checking.
    /// </summary>
    [Serializable]
    public class PermissionException : Exception
    {
        /// <summary>
        /// Gets permission code or role name that being checked.
        /// </summary>
        public string PermissionCode { get; private set; }

        /// <summary>
        /// Gets user name that request access to a resource.
        /// </summary>
        public string UserName { get; private set; }

        public string Title
        {
            get { return "Insufficient Permission"; }
        }

        public string Description
        {
            get { return "you either trying to access resource when you didn't login or you don't have sufficient permission."; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionException"/> class.
        /// </summary>
        public PermissionException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionException"/> class with specified user name, and permission code or role name.
        /// </summary>
        /// <param name="userName">user name that requesting access to resource.</param>
        /// <param name="permissionCode">permission code or role name that being checked.</param>
        public PermissionException(string userName, string permissionCode)
            : base(string.Format("You [{0}] must be logged in, and have [{1}] permission to complete!", userName, permissionCode))
        {
            this.UserName = userName;
            this.PermissionCode = permissionCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionException"/> class with specified user name, permission code or role name, error message, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="userName">user name that requesting access to resource.</param>
        /// <param name="permissionCode">permission code or role name that being checked.</param>
        /// <param name="message">custom message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public PermissionException(string userName, string permissionCode, string message, Exception inner)
            : base(message, inner)
        {
            this.UserName = userName;
            this.PermissionCode = permissionCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected PermissionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            PermissionCode = info.GetString(nameof(PermissionCode));
            UserName = info.GetString(nameof(UserName));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(PermissionCode), PermissionCode);
            info.AddValue(nameof(UserName), UserName);
        }
    }
}
