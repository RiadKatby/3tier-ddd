using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RefactorName.Core
{
    [Serializable]
    public class PermissionException : Exception
    {
        public ErrorCode ErrorCode { get; private set; }
        public string PermissionCode { get; private set; }

        public PermissionException() { }
        public PermissionException(string message) : base(message) { }
        public PermissionException(string message, ErrorCode errorCode) : base(message) { this.ErrorCode = errorCode; }
        public PermissionException(string message, string permissionName) : base(message) { }
        public PermissionException(string message, string permissionName, ErrorCode errorCode) : base(message) { this.ErrorCode = errorCode; }
        public PermissionException(string message, string permissionName, Exception inner) : base(message, inner) { }
        public PermissionException(string message, string permissionName, Exception inner, ErrorCode errorCode) : base(message, inner) { this.ErrorCode = errorCode; }
        protected PermissionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
        protected PermissionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context,
          ErrorCode errorCode)
            : base(info, context) { this.ErrorCode = errorCode; }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
