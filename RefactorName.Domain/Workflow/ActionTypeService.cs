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
    public class ActionTypeService
    {
        //public static
        public static ActionTypeService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static ActionTypeService()
        {
            Obj = new ActionTypeService();
        }

        private ActionTypeService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public ActionType FindById(int ActionTypeID)
        {
            if (ActionTypeID == 0)
                throw new ArgumentNullException("ActionTypeID", "must not be null.");

            var constraints = new QueryConstraints<ActionType>()
                .Where(p => p.ActionTypeId == ActionTypeID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<ActionType> GetAllActionTypes()
        {
            var constraints = new QueryConstraints<ActionType>().SortByDescending(c => c.ActionTypeId);
            return queryRepository.Find(constraints);
        }
    }
}
