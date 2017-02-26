using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.SqlServerRepository
{
    internal static class ThrowHelper
    {
        public static RepositoryException ReThrow(Exception ex)
        {
            SqlException sqlEx = TryExtractException<SqlException>(ex);
            if (sqlEx != null)
            {
                string message = string.Empty;
                ErrorTypeEnum error = ErrorTypeEnum.None;

                switch (sqlEx.Number)
                {
                    case (2):
                    case (53):
                        //_error = DataAccessErrorType.NetworkAddressNotFound;
                        break;
                    case (4060):
                        //_error = DataAccessErrorType.InvalidDatabase;
                        break;
                    case (18452):
                    case (18456):
                        error = ErrorTypeEnum.LoginFailed;
                        message = "خطأ في المصادقة مع مخدم قواعد البيانات";
                        break;
                    case (10054):
                        //_error = DataAccessErrorType.ConnectionRefused;
                        break;
                    case (547):
                    case (2627):
                    case (2601):
                        error = ErrorTypeEnum.DuplicateValue;
                        message = "يجب أن لا يتكرر الحقل الفريد";
                        break;
                    default:
                        break;

                }

                if (sqlEx.Class == 20)
                {
                    error = ErrorTypeEnum.ConnectionFailed;
                    message = "اسم مخدم قاعدة البيانات غير صحيح أو لا يمكن الوصول إليه";
                }

                return new RepositoryException(message, error, ex, ErrorCode.DatabaseError);
            }
            else if (ex is InvalidOperationException)
            {
                return new RepositoryException(ex.Message, ErrorTypeEnum.None, ex, ErrorCode.DatabaseInvalidOperation);
            }
            else if (ex is DbUpdateConcurrencyException)
            {
                return new RepositoryException("تم تعديل الكائن من قبل مستخدم آخر, يرجى تحديث المستعرض والمحاولة مرة آخرى", ex, ErrorCode.ModifiedbyAnotherUserCheckUpdates);
            }
            else if (ex is ModelValidationException)
            {
                return new RepositoryException("يجب التأكد من أن قواعد التأكد من الصحة صحيحة", ErrorTypeEnum.ValidationError, ex, ErrorCode.DatabaseInvalidData);
            }

            return null;
        }

        public static T TryExtractException<T>(Exception ex)
            where T : Exception
        {
            while (ex != null && !(ex is T))
                ex = ex.InnerException;

            return ex as T;
        }
    }
}
