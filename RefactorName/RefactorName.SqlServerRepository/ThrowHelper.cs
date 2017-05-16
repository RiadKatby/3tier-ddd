using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using RefactorName.Core;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace RefactorName.SqlServerRepository
{
    internal static class ThrowHelper
    {
        private static string GetEntityName(object entity)
        {
            DisplayNameAttribute displayName = entity.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
            if (displayName != null)
                return displayName.DisplayName;
            else
                return entity.GetType().Name;
        }

        private static BusinessRuleException TryCreateDeleteViolated()
        {
            return new BusinessRuleException();
        }

        private static BusinessRuleException TryCreateUpdateViolated()
        {
            return new BusinessRuleException();
        }

        private static BusinessRuleException TryCreateUniqnessViolated()
        {
            return new BusinessRuleException();
        }

        public static Exception ReThrow<T>(Exception ex)
        {
            SqlException sqlEx = TryExtractException<SqlException>(ex);
            DbUpdateException dbEx = TryExtractException<DbUpdateException>(ex);

            //string entityNames = string.Join(", ", dbEx.Entries.Select(x => GetEntityName(x.Entity)));

            if (sqlEx != null)
            {
                string exMessage = sqlEx.Message;

                string message = string.Empty;
                ErrorTypeEnum error = ErrorTypeEnum.None;
                string columnName = string.Empty;
                string tableName = string.Empty;
                string indexName = string.Empty;
                MatchCollection coll;

                switch (sqlEx.Number)
                {
                    case (2):
                    case (53):
                        //_error = DataAccessErrorType.NetworkAddressNotFound;
                        break;
                    case (547):
                        if (exMessage.Contains("DELETE"))
                        {
                            Match match = Regex.Match(exMessage, @"\'([^']*)\'");
                            coll = Regex.Matches(exMessage, "\"[^\"]*\"");
                            if (match.Success)
                                columnName = match.Value.Substring(1, match.Value.Length - 2);

                            if (coll.Count == 3)
                                tableName = coll[2].Value.Substring(1, coll[2].Value.Length - 2);
                        }
                        error = ErrorTypeEnum.DeleteReferencedRecord;

                        string tableNameCaption = tableName;
                        string entityNameCaption = typeof(T).Name;

                        if (string.IsNullOrEmpty(tableNameCaption) || string.IsNullOrEmpty(entityNameCaption))
                            message = "Can not delete this element because some element is related to it.";
                        else
                            message = string.Format("Can not delete because {0} is depending on {1} , which you are deleting.", tableNameCaption, entityNameCaption);

                        return new BusinessRuleException(message, "Master Entity Deletion With Slave Entities", typeof(T).Name, sqlEx);
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

                return new RepositoryException(typeof(T).Name, error, message, ex);
            }
            else if (ex is InvalidOperationException)
            {
                return new RepositoryException(typeof(T).Name, ErrorTypeEnum.None, ex.Message, ex);
            }
            else if (ex is DbUpdateConcurrencyException)
            {
                return new RepositoryException(typeof(T).Name, ErrorTypeEnum.ConcurrencyCheckFailed, "تم تعديل الكائن من قبل مستخدم آخر, يرجى تحديث المستعرض والمحاولة مرة آخرى", ex);
            }
            else if (ex is ModelValidationException)
            {
                return new RepositoryException(typeof(T).Name, ErrorTypeEnum.ValidationError, "يجب التأكد من أن قواعد التأكد من الصحة صحيحة", ex);
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
