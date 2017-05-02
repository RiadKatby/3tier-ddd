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
    public class ActivityTargetService
    {
        //public static
        public static ActivityTargetService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static ActivityTargetService()
        {
            Obj = new ActivityTargetService();
        }

        private ActivityTargetService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public ActivityTarget Update(ActivityTarget entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Activity Target", "must not be null.");

            ActivityTarget tempActivityTarget;

            if (entity.ActivityTargetId > 0)
            {
                tempActivityTarget = repository.Update(entity);
            }
            else
            {
                tempActivityTarget = repository.Create(entity);
            }
            if (tempActivityTarget != null)
                return tempActivityTarget;

            return null;
        }
        public bool Delete(ActivityTarget entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Activity Target", "must not be null.");

            bool isDeleted = repository.Delete(entity);

            return isDeleted;
        }

        //public IQueryResult<ActivityTarget> Find(ActivityTargetSearchCriteria ActivityTargetSearchCriteria)
        //{
        //    if (ActivityTargetSearchCriteria == null)
        //        throw new ArgumentNullException("ActivityTargetSearchCriteria", "must not be null.");

        //    var constraints = new QueryConstraints<ActivityTarget>().IncludePath(p => p.Name).SortByDescending(p => p.ActivityTargetId)

        //        .Page(ActivityTargetSearchCriteria.PageNumber, ActivityTargetSearchCriteria.PageSize).Where(p => p.ActivityTargetId > 0);


        //    if (!string.IsNullOrEmpty(ActivityTargetSearchCriteria.ActivityTargetName))
        //        constraints.AndAlso(p => p.Name.Contains(ActivityTargetSearchCriteria.ActivityTargetName));

        //    return queryRepository.Find(constraints);
        //}

        public ActivityTarget FindById(int ActivityTargetID)
        {
            if (ActivityTargetID == 0)
                throw new ArgumentNullException("ActivityTargetID", "must not be null.");

            var constraints = new QueryConstraints<ActivityTarget>()
                .Where(p => p.ActivityTargetId == ActivityTargetID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<ActivityTarget> GetActivityTargetesByProcessId(int processId)
        {
            var constraints = new QueryConstraints<ActivityTarget>()
                .SortByDescending(c => c.ActivityTargetId)
                .IncludePath(c => c.Activity)
                .IncludePath(c => c.Activity.Process)
                .IncludePath(c => c.Group)
                .IncludePath(c => c.Group.Process)
                .IncludePath(c=>c.Target);

            constraints.AndAlso(c => c.Group.ProcessId == processId);
            constraints.AndAlso(c => c.Activity.ProcessId == processId);
            return queryRepository.Find(constraints);
        }

        public IQueryResult<ActivityTarget> FindByGroupId(int groupId)
        {
            var constraints = new QueryConstraints<ActivityTarget>()
                .SortByDescending(c => c.ActivityTargetId);
                

            constraints.AndAlso(c => c.GroupId == groupId);
            return queryRepository.Find(constraints);
        }

        public IQueryResult<ActivityTarget> FindByActivityId(int activityId)
        {
            var constraints = new QueryConstraints<ActivityTarget>()
                .SortByDescending(c => c.ActivityTargetId);


            constraints.AndAlso(c => c.ActivityId == activityId);
            return queryRepository.Find(constraints);
        }
    }
}
