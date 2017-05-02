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
    public class ActionTargetService
    {
        //public static
        public static ActionTargetService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static ActionTargetService()
        {
            Obj = new ActionTargetService();
        }

        private ActionTargetService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public ActionTarget Create(ActionTarget entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Action Target", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            var tempEntity = repository.Create(entity);

            if (tempEntity != null)
                return tempEntity;

            return null;
        }
        public ActionTarget Update(ActionTarget entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Action Target", "must not be null.");

            ActionTarget tempActionTarget;

            if (entity.ActionTargetId > 0)
            {
                tempActionTarget = repository.Update(entity);
            }
            else
            {
                tempActionTarget = repository.Create(entity);
            }
            if (tempActionTarget != null)
                return tempActionTarget;

            return null;
        }
        public bool Delete(ActionTarget entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Action Target", "must not be null.");

            bool isDeleted = repository.Delete(entity);

            return isDeleted;
        }
        //public IQueryResult<ActionTarget> Find(ActionTargetSearchCriteria ActionTargetSearchCriteria)
        //{
        //    if (ActionTargetSearchCriteria == null)
        //        throw new ArgumentNullException("ActionTargetSearchCriteria", "must not be null.");

        //    var constraints = new QueryConstraints<ActionTarget>().IncludePath(p => p.Name).SortByDescending(p => p.ActionTargetId)

        //        .Page(ActionTargetSearchCriteria.PageNumber, ActionTargetSearchCriteria.PageSize).Where(p => p.ActionTargetId > 0);


        //    if (!string.IsNullOrEmpty(ActionTargetSearchCriteria.ActionTargetName))
        //        constraints.AndAlso(p => p.Name.Contains(ActionTargetSearchCriteria.ActionTargetName));

        //    return queryRepository.Find(constraints);
        //}

        public ActionTarget FindById(int ActionTargetID)
        {
            if (ActionTargetID == 0)
                throw new ArgumentNullException("ActionTargetID", "must not be null.");

            var constraints = new QueryConstraints<ActionTarget>()
                .Where(p => p.ActionTargetId == ActionTargetID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<ActionTarget> GetAllActionTargetes()
        {
            var constraints = new QueryConstraints<ActionTarget>().SortByDescending(c => c.ActionTargetId);
            return queryRepository.Find(constraints);
        }

        public IQueryResult<ActionTarget> GetActionTargetesByProcessId(int processId)
        {
            var constraints = new QueryConstraints<ActionTarget>()
                .SortByDescending(c => c.ActionTargetId)
                .IncludePath(c => c.Action)
                .IncludePath(c => c.Action.Process)
                .IncludePath(c => c.Group)
                .IncludePath(c => c.Group.Process)
                .IncludePath(c => c.Target);

            constraints.AndAlso(c => c.Group.ProcessId == processId);
            constraints.AndAlso(c => c.Action.ProcessId == processId);
            return queryRepository.Find(constraints);
        }

        public IQueryResult<ActionTarget> FindByGroupId(int groupId)
        {
            var constraints = new QueryConstraints<ActionTarget>()
               .SortByDescending(c => c.ActionTargetId);
                           
            constraints.AndAlso(c => c.GroupId == groupId);

            return queryRepository.Find(constraints);
        }
        public IQueryResult<ActionTarget> FindByActionId(int actionId)
        {
            var constraints = new QueryConstraints<ActionTarget>()
               .SortByDescending(c => c.ActionTargetId);

            constraints.AndAlso(c => c.ActionId == actionId);

            return queryRepository.Find(constraints);
        }
    }
}
