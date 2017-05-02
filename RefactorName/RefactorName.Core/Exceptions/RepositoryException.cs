using System;
using System.Collections.Generic;
using System.Linq;
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
        ValidationError
    }

    [Serializable]
    public class RepositoryException : Exception
    {
        public ErrorCode ErrorCode { get; private set; }
        public ErrorTypeEnum ErrorType { get; private set; }

        public string BusinessEntityName { get; private set; }

        public RepositoryException()
        {
        }

        public RepositoryException(string message)
            : base(message)
        {
        }
        public RepositoryException(string message, ErrorCode errorCode)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public RepositoryException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public RepositoryException(string message, Exception inner, ErrorCode errorCode)
            : base(message, inner)
        {
            this.ErrorCode = errorCode;
        }

        public RepositoryException(string message, string bizEntityName, Exception inner)
            : base(message, inner)
        {
            this.BusinessEntityName = bizEntityName;
        }
        public RepositoryException(string message, string bizEntityName, Exception inner, ErrorCode errorCode)
            : base(message, inner)
        {
            this.ErrorCode = errorCode;
            this.BusinessEntityName = bizEntityName;
        }

        public RepositoryException(string message, ErrorTypeEnum errType, Exception inner)
            : base(message, inner)
        {
            this.ErrorType = errType;
        }
        public RepositoryException(string message, ErrorTypeEnum errType, Exception inner, ErrorCode errorCode)
            : base(message, inner)
        {
            this.ErrorCode = errorCode;
            this.ErrorType = errType;
        }

        protected RepositoryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
