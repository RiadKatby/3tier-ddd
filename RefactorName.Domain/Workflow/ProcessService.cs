using RefactorName.Core;
using RefactorName.Core.SearchEntities;
using RefactorName.RepositoryInterface;
using RefactorName.RepositoryInterface.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Domain.Workflow
{
    public class ProcessService
    {
        //public static
        public static ProcessService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static ProcessService()
        {
            Obj = new ProcessService();
        }

        private ProcessService()
        {
            queryRepository = RepositoryFactory.CreateQueryRepository();
            repository = RepositoryFactory.CreateRepository();
        }

        public Process FindByIdToDelete(int processID)
        {
            var constraints = new QueryConstraints<Process>()
                .Where(p => p.ProcessId == processID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public bool Delete(Process entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Process", "must not be null.");

            return repository.Delete(entity);

        }

        public Process Create(Process entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Process", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            var tempEntity = repository.Create(entity);

            if (tempEntity != null)
                return tempEntity;

            return null;
        }
        public Process Update(Process entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Process", "must not be null.");

            Process tempProcess;
            if (entity.ProcessId > 0)
            {
                tempProcess = repository.Update(entity);
            }
            else
            {
                tempProcess = repository.Create(entity);
            }
            if (tempProcess == null)
                tempProcess = entity;
            return tempProcess;
            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);


            //if (tempEntity != null)
            //    return tempEntity;

            //return null;
        }

        public IQueryResult<Process> Find(ProcessSearchCriteria processSearchCriteria)
        {
            if (processSearchCriteria == null)
                throw new ArgumentNullException("processSearchCriteria", "must not be null.");

            var constraints = new QueryConstraints<Process>().SortBy(p => p.Name).SortBy(p => p.ProcessId)

                .Page(processSearchCriteria.PageNumber, processSearchCriteria.PageSize).Where(p => p.ProcessId > 0);


            if (!string.IsNullOrEmpty(processSearchCriteria.Name))
                constraints.AndAlso(p => p.Name.Contains(processSearchCriteria.Name));


            return queryRepository.Find(constraints);
        }

        public Process FindById(int processID)
        {
            if (processID < 0)
                throw new ArgumentNullException("processID", "must not be null.");

            if (processID == 0)
            {
                return new Process();
            }

            var constraints = new QueryConstraints<Process>()
                .IncludePath(p => p.Users)
                .IncludePath("Users.Groups")
                .IncludePath("Users.Groups.Process")
                .IncludePath("Users.Groups.Process.Actions")
                .IncludePath("Users.Groups.Process.States")
                .IncludePath("Users.Groups.Process.Activities")
                .IncludePath("Users.Groups.Process.Transitions")


                .IncludePath(p => p.States)
                .IncludePath("States.StateType")
                .IncludePath("States.Activities")
                .IncludePath("States.Activities.ActivityType")


                .IncludePath(p => p.Activities)
                .IncludePath("Activities.ActivityType")
                .IncludePath("Activities.Transitions")
                .IncludePath("Activities.Transitions.Process")
                .IncludePath("Activities.States")
                .IncludePath("Activities.States.StateType")
                .IncludePath("Activities.States.Process")

                .IncludePath(s => s.Actions)
                .IncludePath("Actions.ActionType")
                .IncludePath("Actions.Transitions")


                .IncludePath(s => s.Transitions)
                .IncludePath("Transitions.CurrentState")
                .IncludePath("Transitions.NextState")
                .IncludePath("Transitions.CurrentState.Process")
                .IncludePath("Transitions.CurrentState.Activities")
                .IncludePath("Transitions.CurrentState.Activities.ActivityType")
                .IncludePath("Transitions.CurrentState.Process.Transitions")
                .IncludePath("Transitions.CurrentState.Process.Activities.ActivityType")
                .IncludePath("Transitions.CurrentState.StateType")
                .IncludePath("Transitions.NextState")
                .IncludePath("Transitions.NextState.Process")
                .IncludePath("Transitions.NextState.Process.Activities")
                .IncludePath("Transitions.NextState.Process.Transitions")
                .IncludePath("Transitions.NextState.Process.Activities.ActivityType")
                .IncludePath("Transitions.Actions")
                .IncludePath("Transitions.Actions.ActionType")
                .IncludePath("Transitions.Activities")
                .IncludePath("Transitions.Activities.ActivityType")

                .IncludePath(p => p.Groups)
                .IncludePath("Groups.Users")

                .Where(p => p.ProcessId == processID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public Process FindByIdForTransition(int processID)
        {
            if (processID < 0)
                throw new ArgumentNullException("processID", "must not be null.");

            if (processID == 0)
            {
                return new Process();
            }

            var constraints = new QueryConstraints<Process>()
                .IncludePath("Transitions")
.IncludePath("Transitions.Actions")
.IncludePath("Transitions.Actions.ActionType")
.IncludePath("Transitions.Actions.Process")
.IncludePath("Transitions.Activities")
.IncludePath("Transitions.Activities.Process")
.IncludePath("Transitions.NextState")
.IncludePath("Transitions.NextState.Process")
.IncludePath("Transitions.NextState.StateType")
.IncludePath("Transitions.CurrentState")
.IncludePath("Transitions.CurrentState.Process")
.IncludePath("Transitions.CurrentState.StateType")
.IncludePath("States")
.IncludePath("States.Activities")
.IncludePath("States.Activities.Process")
.IncludePath("States.StateType")
.IncludePath("Activities")
.IncludePath("Activities.States")
.IncludePath("Activities.States.Process")
.IncludePath("Activities.States.StateType")
.IncludePath("Activities.Transitions")
.IncludePath("Activities.Transitions.Process")
.IncludePath("Activities.ActivityType")
.IncludePath("Actions")
.IncludePath("Actions.Transitions")
.IncludePath("Actions.Transitions.Process")
.IncludePath("Actions.ActionType")
.IncludePath("Groups")
.IncludePath("Groups.Users")
.IncludePath("Users")

                .Where(p => p.ProcessId == processID);

            return queryRepository.SingleOrDefault(constraints);
        }
        public Process FindProcessActions(int processID)
        {
            if (processID <= 0)
                throw new ArgumentNullException("processID", "must not be null.");

            var constraints = new QueryConstraints<Process>().IncludePath(p => p.Actions)
                .Where(p => p.ProcessId == processID);

            return queryRepository.SingleOrDefault(constraints);
        }
        public Process FindProcessTransitions(int processID)
        {
            if (processID <= 0)
                throw new ArgumentNullException("processID", "must not be null.");

            var constraints = new QueryConstraints<Process>().IncludePath(p => p.Transitions)
                .Where(p => p.ProcessId == processID);

            return queryRepository.SingleOrDefault(constraints);
        }
        public Process FindProcessActivities(int processID)
        {
            if (processID <= 0)
                throw new ArgumentNullException("processID", "must not be null.");

            var constraints = new QueryConstraints<Process>().IncludePath(p => p.Activities)
                .Where(p => p.ProcessId == processID);

            return queryRepository.SingleOrDefault(constraints);
        }
        public Process FindProcessAdmins(int processID)
        {
            if (processID <= 0)
                throw new ArgumentNullException("processID", "must not be null.");

            var constraints = new QueryConstraints<Process>()/*.IncludePath(p => p.Admins)*/
                .Where(p => p.ProcessId == processID);

            return queryRepository.SingleOrDefault(constraints);
        }
        public Process FindProcessFields(int processID)
        {
            if (processID <= 0)
                throw new ArgumentNullException("processID", "must not be null.");

            var constraints = new QueryConstraints<Process>().IncludePath(p => p.Fields)
                .Where(p => p.ProcessId == processID);

            return queryRepository.SingleOrDefault(constraints);
        }
        public Process FindProcessGroups(int processID)
        {
            if (processID <= 0)
                throw new ArgumentNullException("processID", "must not be null.");

            var constraints = new QueryConstraints<Process>().IncludePath(p => p.Groups)
                .Where(p => p.ProcessId == processID);

            return queryRepository.SingleOrDefault(constraints);
        }
        public Process FindProcessStates(int processID)
        {
            if (processID <= 0)
                throw new ArgumentNullException("processID", "must not be null.");

            var constraints = new QueryConstraints<Process>().IncludePath(p => p.States)
                .Where(p => p.ProcessId == processID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<Process> GetAllProcesses()
        {
            var constraints = new QueryConstraints<Process>()
                .SortBy(p => p.Name)
                .SortByDescending(c => c.ProcessId);
            return queryRepository.Find(constraints);
        }
    }
}
