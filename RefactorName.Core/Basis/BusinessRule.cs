using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Basis
{
    /// <summary>
    /// Encapsulate a specific business rule information.
    /// </summary>
    public class BusinessRule
    {
        /// <summary>
        /// Gets business rule name, that must be coded to identify where this business rule is violated.
        /// </summary>
        public string RuleName { get; private set; }

        /// <summary>
        /// Gets business rule messages, that describe the business rule friendly.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the BusinessRule with specified ruleName, and message.
        /// </summary>
        /// <param name="ruleName">business rule name or code.</param>
        /// <param name="message">human readable business rule message.</param>
        public BusinessRule(string ruleName, string message)
        {
            RuleName = ruleName;
            Message = message;
        }
    }
}
