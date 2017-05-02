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
    public class TransitionService
    {
        //public static
        public static TransitionService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static TransitionService()
        {
            Obj = new TransitionService();
        }

        private TransitionService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public Transition Create(Transition entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Transition", "must not be null.");

            Transition tempEntity;
            if (entity.TransitionId > 0)
                tempEntity = repository.Update(entity);
            else
                tempEntity = repository.Create(entity);

            if (tempEntity != null)
                return tempEntity;

            return null;
        }
        public Transition Update(Transition entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Transition", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            var tempEntity = repository.Update<Transition>(entity);

            if (tempEntity != null)
                return tempEntity;

            return null;
        }
        public bool Delete(Transition entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Transition", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            var isDeleted = repository.Delete<Transition>(entity);

            

            return isDeleted;
        }
        public IQueryResult<Transition> Find(TransitionSearchCriteria TransitionSearchCriteria)
        {
            if (TransitionSearchCriteria == null)
                throw new ArgumentNullException("TransitionSearchCriteria", "must not be null.");

            var constraints = new QueryConstraints<Transition>().SortByDescending(p => p.TransitionId)
                .IncludePath(t => t.CurrentState).IncludePath(t=>t.NextState).IncludePath(t=>t.Process)
                .Page(TransitionSearchCriteria.PageNumber, TransitionSearchCriteria.PageSize).Where(p => p.TransitionId > 0);


            if (TransitionSearchCriteria.ProcessId!=null&& TransitionSearchCriteria.ProcessId>0)
                constraints.AndAlso(t => t.ProcessId==TransitionSearchCriteria.ProcessId);

            if (TransitionSearchCriteria.CurrentStateId != null && TransitionSearchCriteria.CurrentStateId > 0)
                constraints.AndAlso(t => t.CurrentState.StateId == TransitionSearchCriteria.CurrentStateId);

            if (TransitionSearchCriteria.NextStateId != null && TransitionSearchCriteria.NextStateId > 0)
                constraints.AndAlso(t => t.NextState.StateId == TransitionSearchCriteria.NextStateId);

            return queryRepository.Find(constraints);
        }

        public Transition FindById(int transitionID)
        {
            if (transitionID == 0)
                throw new ArgumentNullException("TransitionID", "must not be null.");

            var constraints = new QueryConstraints<Transition>()
                //.IncludePath(t=>t.Actions)
                //.IncludePath(t=>t.Process)
                //.IncludePath(t=>t.Process.States)
                //.IncludePath("Process.States.StateType")
                //.IncludePath(t => t.Process.Activities)
                //.IncludePath("Process.Activities.ActivityType")
                //.IncludePath(t=>t.CurrentState)
                //.IncludePath(t => t.CurrentState.Process)
                //.IncludePath(t=>t.CurrentState.Process.Activities)
                //.IncludePath(t => t.CurrentState.Process.Transitions)
                //.IncludePath("CurrentState.Process.Activities.ActivityType")
                //.IncludePath(t=>t.CurrentState.StateType)
                //.IncludePath(t => t.NextState)
                //.IncludePath(t => t.NextState.Process)
                //.IncludePath(t => t.NextState.Process.Activities)
                //.IncludePath(t => t.NextState.Process.Transitions)
                //.IncludePath("NextState.Process.Activities.ActivityType")

                //.IncludePath(t => t.NextState.StateType)
                .Where(t => t.TransitionId == transitionID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<Transition> GetAllTransitiones()
        {
            var constraints = new QueryConstraints<Transition>()
                .IncludePath(t => t.Actions)
                .IncludePath(t => t.Activities)
                .IncludePath(t=>t.CurrentState)
                .IncludePath(t => t.NextState)
                .SortByDescending(c => c.TransitionId);
            return queryRepository.Find(constraints);
        }

        public IQueryResult<Transition> FindByProcessId(int processId)
        {
            if (processId == 0)
                throw new ArgumentNullException("TransitionID", "must not be null.");

            var constraints = new QueryConstraints<Transition>()
                .IncludePath(t=>t.Process)
                .IncludePath(t=>t.CurrentState)
                .IncludePath(t => t.NextState)
                .Where(t => t.ProcessId == processId);

            return queryRepository.Find(constraints);
        }
    }
}
