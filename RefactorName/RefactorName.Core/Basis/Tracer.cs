using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// Logging all events to the ETW semantically.
    /// </summary>
    [EventSource(Name = "RefactorName-Template-Tracer")]
    public sealed class Tracer : EventSource
    {
        private static readonly Lazy<Tracer> instance = new Lazy<Tracer>(() => new Tracer());

        private Tracer() { }

        /// <summary>
        /// Gets singleton logger instance of <see cref="Tracer"/> class.
        /// </summary>
        public static Tracer Log
        {
            get { return instance.Value; }
        }

        /// <summary>
        /// Logs Verbose that new business entity is created and persisted, this method must be called in Domain Services Classes.
        /// </summary>
        /// <param name="entityName">Business entity name.</param>
        /// <param name="entityId">The generated Id of created business entity.</param>
        [Event(1, Level = EventLevel.Verbose)]
        public void EntityCreated(string entityName, int entityId)
        {
            if (IsEnabled())
            {
                WriteEvent(1, entityName, entityId);
            }
        }

        /// <summary>
        /// Logs Verbose that existed entity is updated and persisted, this method must be called in Domain Services Classes.
        /// </summary>
        /// <param name="entityName">business entity name.</param>
        /// <param name="entityId">the Id of updated business entity.</param>
        [Event(2, Level = EventLevel.Verbose)]
        public void EntityUpdated(string entityName, int entityId)
        {
            if (IsEnabled())
            {
                WriteEvent(2, entityName, entityId);
            }
        }

        /// <summary>
        /// Logs Verbose that existed entity is deleted, this method must be called in Domain Services Classes.
        /// </summary>
        /// <param name="entityName">business entity name.</param>
        /// <param name="entityId">entity Id that is deleted.</param>
        [Event(3, Level = EventLevel.Verbose)]
        public void EntityDeleted(string entityName, int entityId)
        {
            if (IsEnabled())
            {
                WriteEvent(3, entityName, entityId);
            }
        }

        /// <summary>
        /// Logs Verbose that a set of entities have retrieved from repository, this method must be called in Domain Services Classes.
        /// </summary>
        /// <param name="entityName">business entity name.</param>
        /// <param name="count">count of business entity that are actually retrieved.</param>
        /// <param name="totalCount">total count of business entities that are existed in Repository.</param>
        [Event(4, Level = EventLevel.Verbose)]
        public void EntitiesRetrieved(string entityName, int count, int totalCount)
        {
            if (IsEnabled())
            {
                WriteEvent(4, entityName, count, totalCount);
            }
        }

        /// <summary>
        /// Logs Verbose that individual business entity has been retrieved from repository, this method must be called in Domain Services Classes.
        /// </summary>
        /// <param name="entityName">business entity name.</param>
        /// <param name="entityId">entity Id that is retrieved.</param>
        [Event(5, Level = EventLevel.Verbose)]
        public void EntityRetrieved(string entityName, int entityId)
        {
            if (IsEnabled())
            {
                WriteEvent(5, entityName, entityId);
            }
        }

        /// <summary>
        /// Logs Informational that a specific business rule is violated, this method must be called in Domain Services Classes.
        /// </summary>
        /// <param name="entityName">business entity name.</param>
        /// <param name="ruleName">business rule code that is violated.</param>
        [Event(6, Level = EventLevel.Informational)]
        public void BusinessRuleViolated(string entityName, string ruleName)
        {
            if (IsEnabled())
            {
                WriteEvent(6, entityName, ruleName);
            }
        }

        /// <summary>
        /// Logs Informational that a specific business entity is not found in repository, this method must be called in Domain Services Classes.
        /// </summary>
        /// <param name="entityName">business entity name.</param>
        /// <param name="entityId"></param>
        /// <param name="fullException"></param>
        [Event(7, Level = EventLevel.Informational)]
        public void EntityNotFound(string entityName, object entityId, string fullException)
        {
            if (IsEnabled())
            {
                WriteEvent(7, entityName, entityId, fullException);
            }
        }


        [Event(8, Level = EventLevel.Error)]
        public void PermissionViolated(string userName, string permissionName, string fullException)
        {
            if (IsEnabled())
            {
                WriteEvent(8, userName, permissionName, fullException);
            }
        }

        [Event(9, Level = EventLevel.Warning)]
        public void EntityNotValied(string entityName, int validationCount)
        {
            if (IsEnabled())
            {
                WriteEvent(9, entityName, validationCount);
            }
        }

        [Event(10, Level = EventLevel.Critical)]
        public void RepositoryFailure(string entityName, string errorType, string fullException)
        {
            if (IsEnabled())
            {
                WriteEvent(10, entityName, errorType, fullException);
            }
        }

        [Event(11, Level = EventLevel.Critical)]
        public void Failure(string fullDetails)
        {
            if (IsEnabled())
            {
                WriteEvent(11, fullDetails);
            }
        }

        [Event(12, Level = EventLevel.Critical)]
        public void ArgumentNull(string paramName, string fullException)
        {
            if (IsEnabled())
            {
                WriteEvent(12, paramName, fullException);
            }
        }

        [Event(13, Level = EventLevel.Critical)]
        public void ArgumentOutOfRange(string paramName, object actualValue, string fullException)
        {
            if (IsEnabled())
            {
                WriteEvent(13, paramName, actualValue, fullException);
            }
        }

        [Event(14, Level = EventLevel.Critical)]
        public void Argument(string paramName, string fullException)
        {
            if (IsEnabled())
            {
                WriteEvent(14, paramName, fullException);
            }
        }
    }
}
