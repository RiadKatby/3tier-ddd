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
    public class ActivityService
    {
        //public static
        public static ActivityService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static ActivityService()
        {
            Obj = new ActivityService();
        }

        private ActivityService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        //public Activity Create(Activity entity)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException("Activity", "must not be null.");

        //    if (entity.Validate() == false)
        //        throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

        //    var tempEntity = repository.Create(entity);

        //    if (tempEntity != null)
        //        return tempEntity;

        //    return null;
        //}

        public IQueryResult<Activity> Find(ActivitySearchCriteria ActivitySearchCriteria)
        {
            if (ActivitySearchCriteria == null)
                throw new ArgumentNullException("ActivitySearchCriteria", "must not be null.");

            var constraints = new QueryConstraints<Activity>().SortByDescending(a => a.ActivityId)
                .IncludePath(a=>a.Process)
                .IncludePath(a=>a.ActivityType)
                .Page(ActivitySearchCriteria.PageNumber, ActivitySearchCriteria.PageSize).Where(a => a.ActivityId > 0);


            if (!string.IsNullOrEmpty(ActivitySearchCriteria.Name))
                constraints.AndAlso(a => a.Name.Contains(ActivitySearchCriteria.Name));

            if (!string.IsNullOrEmpty(ActivitySearchCriteria.Description))
                constraints.AndAlso(a => a.Description.Contains(ActivitySearchCriteria.Description));

            if (ActivitySearchCriteria.ActivityTypeId!=null&& ActivitySearchCriteria.ActivityTypeId>0)
                constraints.AndAlso(a => a.ActivityType.ActivityTypeId==ActivitySearchCriteria.ActivityTypeId);

            if (ActivitySearchCriteria.ProcessId != null && ActivitySearchCriteria.ProcessId > 0)
                constraints.AndAlso(a => a.ProcessId == ActivitySearchCriteria.ProcessId);

            return queryRepository.Find(constraints);
        }

        public Dictionary<string, string> GetAllActivitiesDictionary(int processId)
        {
            var result = new Dictionary<string, string>();

            var constraints = new QueryConstraints<Activity>()
                .Page(1, int.MaxValue)
                .Where(c => true)
                .AndAlso(c=>c.ProcessId == processId);

            foreach (Activity user in queryRepository.Find(constraints).Items.ToList())
            {
                result.Add(user.ActivityId.ToString(), user.Name);
            }

            return result;
        }        

        public Dictionary<string, string> GetAllUsersDictionary()
        {
            var result = new Dictionary<string, string>();

            var constraints = new QueryConstraints<User>()
                .Page(1, int.MaxValue)
                .Where(c => true);

            User test = queryRepository.Find(constraints).Items.First();
            foreach (User user in queryRepository.Find(constraints).Items.ToList())
            {
                result.Add(user.Id.ToString(), user.FullName);
            }

            return result;
        }

        public Activity FindById(int ActivityID)
        {
            if (ActivityID == 0)
                throw new ArgumentNullException("ActivityID", "must not be null.");

            var constraints = new QueryConstraints<Activity>()
                .Where(p => p.ActivityId == ActivityID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<Activity> GetAllActivityes()
        {
            var constraints = new QueryConstraints<Activity>()
                .IncludePath(a => a.ActivityType)
                .SortByDescending(c => c.ActivityId);
            return queryRepository.Find(constraints);
        }

        public IQueryResult<Activity> FindByProcessId(int processId)
        {
            if (processId == 0)
                throw new ArgumentNullException("processId", "must not be null.");

            var constraints = new QueryConstraints<Core.Activity>()
                .IncludePath(p => p.Process)
                .Where(p => p.ProcessId == processId);

            return queryRepository.Find(constraints);
        }
        public bool Delete(Activity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("State", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            return repository.Delete<Activity>(entity);
        }
    }
}
