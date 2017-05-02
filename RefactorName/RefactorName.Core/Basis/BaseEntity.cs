using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RefactorName.Core
{
    public class BaseEntity : IEntity, IDataErrorInfo
    {

        #region IDataErrorInfo Members

        /// <summary>
        /// Determines if current business object has no validation issue or error.
        /// </summary>
        [NotMapped]
        public bool IsValid { get; protected set; }

        /// <summary>
        /// Represents all the validation result of current business object.
        /// This property is influenced by Validate method call.
        /// </summary>
        [NotMapped]
        public ObservableCollection<ValidationResult> ValidationResults { get; protected set; }

        /// <summary>
        /// Validates all properties of current business object.
        /// This method influence IsValid, and ValidationResults properties.
        /// </summary>
        public virtual bool Validate()
        {
            ValidationResults = new ObservableCollection<ValidationResult>();

            return IsValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(this, new ValidationContext(this, null, null), ValidationResults, true);
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// Invoking this properties will influence IsValid, and ValidationResults properties.
        /// </summary>
        [NotMapped]
        public string Error
        {
            get
            {
                Validate();

                var result = from x in ValidationResults
                             from y in x.ErrorMessage
                             select y;

                return string.Join(Environment.NewLine, result);
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// Invoking this indexer will influence IsValid, and ValidationResults properties.
        /// </summary>
        /// <param name="propertyName">The name of the property whose error message to get.</param>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        public string this[string propertyName]
        {
            get
            {
                Validate();

                ValidationResult result = (from x in ValidationResults
                                           from y in x.MemberNames
                                           where y == propertyName
                                           select x).FirstOrDefault();

                return (result != null) ? result.ErrorMessage : string.Empty;
            }
        }

        #endregion
    }
}
