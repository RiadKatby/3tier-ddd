using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    [EventSource(Name = "RefactorName-Template-Tracer")]
    public sealed class Tracer : EventSource
    {
        private static readonly Lazy<Tracer> instance = new Lazy<Tracer>(() => new Tracer());

        private Tracer() { }

        public static Tracer Log
        {
            get { return instance.Value; }
        }

        [Event(1, Level = EventLevel.Informational)]
        public void EntityCreated(string entityName, int entityId)
        {
            if (IsEnabled())
            {
                WriteEvent(1, entityName, entityId);
            }
        }

        [Event(2, Level = EventLevel.Informational)]
        public void EntityUpdated(string entityName, int entityId)
        {
            if (IsEnabled())
            {
                WriteEvent(2, entityName, entityId);
            }
        }

        [Event(3, Level = EventLevel.Informational)]
        public void EntityDeleted(string entityName, int entityId)
        {
            if (IsEnabled())
            {
                WriteEvent(3, entityName, entityId);
            }
        }

        [Event(4, Level = EventLevel.Informational)]
        public void EntitiesRetrieved(string entityName, int count, int totalCount)
        {
            if (IsEnabled())
            {
                WriteEvent(4, entityName, count, totalCount);
            }
        }

        [Event(5, Level = EventLevel.Informational)]
        public void EntityRetrieved(string entityName, int entityId)
        {
            if (IsEnabled())
            {
                WriteEvent(5, entityName, entityId);
            }
        }

        [Event(6, Level = EventLevel.Warning)]
        public void BusinessRuleViolated(string entityName, string ruleName)
        {
            if (IsEnabled())
            {
                WriteEvent(6, entityName, ruleName);
            }
        }

        [Event(7, Level = EventLevel.Error)]
        public void EntityNotFound(string entityName, object entityId)
        {
            if (IsEnabled())
            {
                WriteEvent(7, entityName, entityId);
            }
        }

        [Event(8, Level = EventLevel.Error)]
        public void PermissionViolated(string permissionName)
        {
            if (IsEnabled())
            {
                WriteEvent(8, permissionName);
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
    }
}
