using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RefactorName.Core
{
    public enum ErrorTypeEnum
    {
        None,

        /// <summary>
        /// خطأ في المصادقة مع مخدم قواعد البيانات
        /// </summary>
        LoginFailed,

        /// <summary>
        /// اسم مخدم قاعدة البيانات غير صحيح أو لا يمكن الوصول إليه
        /// </summary>
        ConnectionFailed,

        /// <summary>
        /// لا يمكن اضافة سجل مرتين
        /// </summary>
        DuplicateValue,
        ValidationError,
        ModifiedbyAnotherUserCheckUpdates,
        DeleteReferencedRecord,
        ConcurrencyCheckFailed,
        NotificationServiceError = 50301,
        DatabaseError = 50102
    }

    /// <summary>
    /// Represents error that occurs during persisting or retrieving information from Repository.
    /// </summary>
    [Serializable]
    public class RepositoryException : Exception
    {
        /// <summary>
        /// Gets generic classification of access repository error.
        /// </summary>
        public ErrorTypeEnum ErrorType { get; private set; }

        /// <summary>
        /// Gets business entity name that being persisted or retrieved.
        /// </summary>
        public string EntityName { get; private set; }

        public string Title
        {
            get { return "Retrieving or Persisting Entity Error"; }
        }

        public string Description
        {
            get { return "You may have an error while accessing external persistence media (Database or Web Service)."; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        public RepositoryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class with specified business entity name, and error type.
        /// </summary>
        /// <param name="entityName">business entity name that being persisted or retrieved.</param>
        /// <param name="errorType">generic classification of errors.</param>
        public RepositoryException(string entityName, ErrorTypeEnum errorType)
            : base(string.Format("Error [{0}] has been occurred while retrieving or persisting [{1}] entity.", entityName, errorType))
        {
            this.EntityName = entityName;
            this.ErrorType = errorType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class with specified business entity name, error type, error message, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="entityName">business entity name that being persisted or retrieved.</param>
        /// <param name="errorType">generic classification of errors.</param>
        /// <param name="message">custom message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public RepositoryException(string entityName, ErrorTypeEnum errorType, string message, Exception inner)
            : base(message, inner)
        {
            this.EntityName = entityName;
            this.ErrorType = errorType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected RepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorType = (ErrorTypeEnum)info.GetValue(nameof(ErrorType), typeof(ErrorTypeEnum));
            EntityName = info.GetString(nameof(EntityName));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ErrorType), ErrorType, typeof(ErrorTypeEnum));
            info.AddValue(nameof(EntityName), EntityName);
        }
    }
}
