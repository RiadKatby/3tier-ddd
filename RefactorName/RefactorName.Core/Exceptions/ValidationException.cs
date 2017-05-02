using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RefactorName.Core
{
    public class ValidationException : Exception
    {
        public ErrorCode ErrorCode { get; private set; }
        public IEnumerable<ValidationResult> ValidationResults { get; private set; }

        public ValidationException() { }

        public ValidationException(string message, IEnumerable<ValidationResult> validationResults)
            : this(message)
        {
            this.ValidationResults = validationResults;
        }
        public ValidationException(string message, IEnumerable<ValidationResult> validationResults, ErrorCode errorCode)
            : this(message)
        {
            this.ValidationResults = validationResults;
            this.ErrorCode = errorCode;
        }

        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, ErrorCode errorCode) : base(message) { this.ErrorCode = errorCode; }

        public ValidationException(string message, Exception inner) : base(message, inner) { }
        public ValidationException(string message, Exception inner, ErrorCode errorCode) : base(message, inner) { this.ErrorCode = errorCode; }

        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
