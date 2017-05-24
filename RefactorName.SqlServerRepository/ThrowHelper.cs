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
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace RefactorName.SqlServerRepository
{
    internal static class ThrowHelper
    {
        /// <summary>
        /// Message of <see cref="InvalidOperationException"/> that is thrown when INSERT, UPDATE, or DELETE master object that has details with NO ACTION is set on relationship.
        /// </summary>
        private const string ConceptualNullKey = "foreign-key properties is non-nullable";

        /// <summary>
        /// Message of <see cref="InvalidOperationException"/> that is thrown when communicate with DbContext and Core is not matched with database schema.
        /// </summary>
        private const string CoreNotMatchedWithDatabaseKey = "context has changed since the database was created";

        /// <summary>
        /// Message of <see cref="SqlException"/> that is thrown when you try to delete principal record that has dependent record in another table.
        /// </summary>
        private const string DeleteConflectKey = "DELETE";

        /// <summary>
        /// Message of <see cref="SqlException"/> that is thrown when you try to write duplicated value in unique column.
        /// </summary>
        private const string UniqueIndexKey = "unique index";

        private static bool TryCreateDeleteViolated(string businessEntityName, Exception ex, ref BusinessRuleException result)
        {
            result = null;

            SqlException sqlEx = TryExtractException<SqlException>(ex);

            if (sqlEx == null)
                return false;

            if (sqlEx.Number != 547 || !sqlEx.Message.Contains(DeleteConflectKey))
                return false;

            string message = $"Coudn't Delete {businessEntityName} Record because a record in another table depend on it.";

            string principleTable = businessEntityName;
            string dependentTable = null;

            MatchCollection matches = Regex.Matches(sqlEx.Message, "\"([^\"]*)\"");
            if (matches.Count == 3) // 3 result must be existed in message "Qualifed Column Name", "Database Name", "Table name"
                dependentTable = matches[2].Value.Replace("dbo.", "").Replace("\"", "");

            if (dependentTable != null)
                message = $"Coudn't Delete {businessEntityName}'s record because a record in {dependentTable} depend on it.";


            result = new BusinessRuleException(message, "ConflectedDelete", businessEntityName, ex);
            return true;
        }

        private static bool TryCreateUniqnessViolated(string businessEntityName, Exception ex, ref BusinessRuleException result)
        {
            result = null;

            SqlException sqlEx = TryExtractException<SqlException>(ex);

            if (sqlEx == null)
                return false;

            if (!sqlEx.Number.In(2601, 2627) || !sqlEx.Message.Contains(UniqueIndexKey))
                return false;

            string message = $"Couldn't add duplicated value in {businessEntityName} table.";
            string table = businessEntityName;
            string duplicatedValue = null;

            MatchCollection matches = Regex.Matches(sqlEx.Message, @"\((.*)\)");
            if (matches.Count == 1)
                duplicatedValue = matches[0].Value;

            if (duplicatedValue != null)
                message = $"Couldn't add duplicated {duplicatedValue} in {table} table.";

            result = new BusinessRuleException(message, "DuplicatedValue", businessEntityName, ex);
            return true;
        }

        private static bool TryCreateValidationViolated(string businessEntityName, Exception ex, ref ValidationException result)
        {
            result = null;

            DbEntityValidationException valEx = ex as DbEntityValidationException;
            if (valEx == null)
                return false;

            var validationResult = from x in valEx.EntityValidationErrors
                                   from y in x.ValidationErrors
                                   select new System.ComponentModel.DataAnnotations.ValidationResult(y.ErrorMessage, new string[] { y.PropertyName });

            string message = $"Trying to write not valid data into {businessEntityName}";

            result = new ValidationException(message, businessEntityName, validationResult, ex);
            return true;
        }

        private static bool TryParsingInvalidOperation(string businessEntityName, Exception ex, ref RepositoryException result)
        {
            result = null;

            InvalidOperationException invalidOpEx = ex as InvalidOperationException;
            if (invalidOpEx == null)
                return false;

            if (ex.Message.Contains(ConceptualNullKey))
                result = new RepositoryException(businessEntityName, ErrorTypeEnum.ConceptualNull, "Delete master object that has details, and database relationship is set to NO ACTION", ex);
            else if (ex.Message.Contains(CoreNotMatchedWithDatabaseKey))
                result = new RepositoryException(businessEntityName, ErrorTypeEnum.CoreNotMatchedWithDatabase, "Core Business Entities have new updates that are not reflected on Database Schema.", ex);
            else
            {
                Debug.WriteLine("TryParsingInvalidOperation Exit with FAILED.");
            }

            return result != null;
        }

        private static bool TryParsingSql(string businessEntityName, Exception ex, ref RepositoryException result)
        {
            result = null;

            SqlException sqlEx = ex as SqlException;
            if (sqlEx == null)
                return false;

            if (sqlEx.Class == 20)
                result = new RepositoryException(businessEntityName, ErrorTypeEnum.ConnectionFailed, "Database server is not accessable or wrong name.", ex);

            if (sqlEx.Number.In(18452, 18456))
                result = new RepositoryException(businessEntityName, ErrorTypeEnum.LoginFailed, "Authentication Failed with Database Server.", ex);

            if (result == null)
                Debug.WriteLine("TryParsingSql Exit with FAILED.");

            return result != null;
        }

        public static Exception ReThrow<T>(Exception ex)
        {
            string businessEntityName = GetName<T>();
            BusinessRuleException buzRule = null;
            ValidationException valEx = null;
            RepositoryException repEx = null;

            if (TryCreateDeleteViolated(businessEntityName, ex, ref buzRule))
                return buzRule;

            if (TryCreateUniqnessViolated(businessEntityName, ex, ref buzRule))
                return buzRule;

            if (TryCreateValidationViolated(businessEntityName, ex, ref valEx))
                return valEx;

            if (TryParsingInvalidOperation(businessEntityName, ex, ref repEx))
                return repEx;

            if (TryParsingSql(businessEntityName, ex, ref repEx))
                return repEx;

            Debug.WriteLine("ReThrow exist with FAILED TO PARSE Exception.");

            return new RepositoryException(businessEntityName, ErrorTypeEnum.None, ex.Message, ex);
        }

        private static string GetName<TBusinessEntity>()
        {
            Type businessEntityType = typeof(TBusinessEntity);

            DisplayNameAttribute displayName = businessEntityType
                .GetCustomAttributes(typeof(DisplayNameAttribute), false)
                .FirstOrDefault() as DisplayNameAttribute;

            if (displayName != null)
                return displayName.DisplayName;
            else
                return businessEntityType.Name;
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
