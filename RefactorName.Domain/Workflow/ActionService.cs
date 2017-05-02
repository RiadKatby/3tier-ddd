using RefactorName.Core;
using RefactorName.RepositoryInterface;
using RefactorName.Core.SearchEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefactorName.RepositoryInterface.Queries;

namespace RefactorName.Domain.Workflow
{
    public class ActionService
    {
        //public static
        public static ActionService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static ActionService()
        {
            Obj = new ActionService();
        }

        private ActionService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        //public Core.Workflow.Action Create(Core.Workflow.Action entity)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException("Action", "must not be null.");

        //    if (entity.Validate() == false)
        //        throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

        //    var tempEntity = repository.Create(entity);

        //    if (tempEntity != null)
        //        return tempEntity;

        //    return null;
        //}

        public IQueryResult<Core.Action> Find(ActionSearchCriteria ActionSearchCriteria)
        {
            if (ActionSearchCriteria == null)
                throw new ArgumentNullException("ActionSearchCriteria", "must not be null.");

            var constraints = new QueryConstraints<Core.Action>().SortByDescending(a => a.ActionId)
                .IncludePath(a => a.ActionType)
                .IncludePath(a => a.Process)
                .Page(ActionSearchCriteria.PageNumber, ActionSearchCriteria.PageSize).Where(a => a.ActionId > 0);


            if (!string.IsNullOrEmpty(ActionSearchCriteria.Name))
                constraints.AndAlso(a => a.Name.Contains(ActionSearchCriteria.Name));

            if (!string.IsNullOrEmpty(ActionSearchCriteria.Description))
                constraints.AndAlso(a => a.Description.Contains(ActionSearchCriteria.Description));

            if (ActionSearchCriteria.ActionTypeId != null && ActionSearchCriteria.ActionTypeId > 0)
                constraints.AndAlso(a => a.ActionTypeId == ActionSearchCriteria.ActionTypeId);

            if (ActionSearchCriteria.ProcessId != null && ActionSearchCriteria.ProcessId > 0)
                constraints.AndAlso(a => a.ProcessId == ActionSearchCriteria.ProcessId);

            if (ActionSearchCriteria.TransitionId != null && ActionSearchCriteria.TransitionId > 0)
                constraints.AndAlso(a => a.Transitions.Any(t => t.TransitionId == ActionSearchCriteria.TransitionId));

            return queryRepository.Find(constraints);
        }

        public Dictionary<string, string> GetAllActionsDictionary(int processId)
        {
            var result = new Dictionary<string, string>();

            var constraints = new QueryConstraints<Core.Action>()
                .Page(1, int.MaxValue)
                .Where(c => true)
                .AndAlso(s => s.ProcessId == processId);

            foreach (Core.Action action in queryRepository.Find(constraints).Items.ToList())
            {
                result.Add(action.ActionId.ToString(), action.Name);
            }

            return result;
        }

        public Core.Action FindById(int actionID)
        {
            if (actionID == 0)
                throw new ArgumentNullException("ActionID", "must not be null.");

            var constraints = new QueryConstraints<Core.Action>()
                .Where(a => a.ActionId == actionID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<Core.Action> GetAllActiones()
        {
            var constraints = new QueryConstraints<Core.Action>()
                .IncludePath(a => a.Transitions)
                .SortByDescending(c => c.ActionId);
            return queryRepository.Find(constraints);
        }

        public IQueryResult<Core.Action> FindByProcessId(int processId)
        {
            if (processId == 0)
                throw new ArgumentNullException("processId", "must not be null.");

            var constraints = new QueryConstraints<Core.Action>()
                .IncludePath(p => p.Process)
                .Where(p => p.ProcessId == processId);

            return queryRepository.Find(constraints);
        }

        public Core.Action Update(Core.Action entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Action", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            var tempEntity = repository.Update<Core.Action>(entity);

            if (tempEntity != null)
                return tempEntity;

            return null;
        }
        public bool Delete(Core.Action entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Action", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            return repository.Delete<Core.Action>(entity);
        }
    }
}
