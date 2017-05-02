using RefactorName.Core;
using RefactorName.RepositoryInterface;
using RefactorName.RepositoryInterface.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Domain.Workflow
{
    public class ActivityTypeService
    {
        //public static
        public static ActivityTypeService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static ActivityTypeService()
        {
            Obj = new ActivityTypeService();
        }

        private ActivityTypeService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public ActivityType FindById(int ActivityTypeID)
        {
            if (ActivityTypeID == 0)
                throw new ArgumentNullException("ActivityTypeID", "must not be null.");

            var constraints = new QueryConstraints<ActivityType>()
                .Where(p => p.ActivityTypeId == ActivityTypeID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<ActivityType> GetAllActivityTypes()
        {
            var constraints = new QueryConstraints<ActivityType>().SortByDescending(c => c.ActivityTypeId);
            return queryRepository.Find(constraints);
        }
    }
}
