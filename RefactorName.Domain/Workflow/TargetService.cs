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
    public class TargetService
    {
        //public static
        public static TargetService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static TargetService()
        {
            Obj = new TargetService();
        }

        private TargetService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public IQueryResult<Target> GetAllTargetes()
        {
            var constraints = new QueryConstraints<Target>().SortByDescending(c => c.TargetId);
            return queryRepository.Find(constraints);
        }

        public Target FindById(int targetId)
        {
            if (targetId == 0)
                throw new ArgumentNullException("targetId", "must not be null.");

            var constraints = new QueryConstraints<Core.Target>()
                .Where(a => a.TargetId == targetId);

            return queryRepository.SingleOrDefault(constraints);
        }
    }
}
