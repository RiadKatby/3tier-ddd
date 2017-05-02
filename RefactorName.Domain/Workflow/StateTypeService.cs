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
    public class StateTypeService
    {
        //public static
        public static StateTypeService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static StateTypeService()
        {
            Obj = new StateTypeService();
        }

        private StateTypeService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public StateType FindById(int StateTypeID)
        {
            if (StateTypeID == 0)
                throw new ArgumentNullException("StateTypeID", "must not be null.");

            var constraints = new QueryConstraints<StateType>()
                .Where(p => p.StateTypeId == StateTypeID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<StateType> GetAllStateTypes()
        {
            var constraints = new QueryConstraints<StateType>().SortByDescending(c => c.StateTypeId);
            return queryRepository.Find(constraints);
        }

        //public StateType Create(StateType entity)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException("StateType", "must not be null.");

        //    if (entity.Validate() == false)
        //        throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

        //    var tempEntity = repository.Create(entity);

        //    if (tempEntity != null)
        //        return tempEntity;

        //    return null;
        //}

        //public IQueryResult<StateType> Find(StateTypeSearchCriteria StateTypeSearchCriteria)
        //{
        //    if (StateTypeSearchCriteria == null)
        //        throw new ArgumentNullException("StateTypeSearchCriteria", "must not be null.");

        //    var constraints = new QueryConstraints<StateType>().IncludePath(p => p.Name).SortByDescending(p => p.StateTypeId)

        //        .Page(StateTypeSearchCriteria.PageNumber, StateTypeSearchCriteria.PageSize).Where(p => p.StateTypeId > 0);


        //    if (!string.IsNullOrEmpty(StateTypeSearchCriteria.StateTypeName))
        //        constraints.AndAlso(p => p.Name.Contains(StateTypeSearchCriteria.StateTypeName));

        //    return queryRepository.Find(constraints);
        //}

        //public StateType FindById(int StateTypeID)
        //{
        //    if (StateTypeID == 0)
        //        throw new ArgumentNullException("StateTypeID", "must not be null.");

        //    var constraints = new QueryConstraints<StateType>()
        //        .Where(p => p.StateTypeId == StateTypeID);

        //    return queryRepository.SingleOrDefault(constraints);
        //}        
    }
}
