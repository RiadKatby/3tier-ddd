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
    public class RequestService
    {
        //public static
        public static RequestService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static RequestService()
        {
            Obj = new RequestService();
        }

        private RequestService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public Request Create(Request entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Request", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            var tempEntity = repository.Create(entity);

            if (tempEntity != null)
                return tempEntity;

            return null;
        }

        public IQueryResult<Request> Find(RequestSearchCriteria RequestSearchCriteria)
        {
            if (RequestSearchCriteria == null)
                throw new ArgumentNullException("RequestSearchCriteria", "must not be null.");

            var constraints = new QueryConstraints<Request>().SortByDescending(p => p.RequestId)

                .Page(RequestSearchCriteria.PageNumber, RequestSearchCriteria.PageSize).Where(r => r.RequestId > 0);


            if (!string.IsNullOrEmpty(RequestSearchCriteria.Title))
                constraints.AndAlso(r => r.Title.Contains(RequestSearchCriteria.Title));

            if (RequestSearchCriteria.UserRequestedId!=null&& RequestSearchCriteria.UserRequestedId>0)
                constraints.AndAlso(r => r.UserRequestedId == RequestSearchCriteria.UserRequestedId);

            if (RequestSearchCriteria.DateRequested != null)
                constraints.AndAlso(r => r.DateRequested == RequestSearchCriteria.DateRequested);

            //if (RequestSearchCriteria.UserRequested != null)
            //    constraints.AndAlso(r => r.UserRequested == RequestSearchCriteria.UserRequested);

            if (RequestSearchCriteria.CurrentState != null)
                constraints.AndAlso(r => r.CurrentState == RequestSearchCriteria.CurrentState);

            return queryRepository.Find(constraints);
        }

        public Request FindById(int RequestID)
        {
            if (RequestID == 0)
                throw new ArgumentNullException("RequestID", "must not be null.");

            var constraints = new QueryConstraints<Request>()
                .Where(p => p.RequestId == RequestID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<Request> GetAllRequestes()
        {
            var constraints = new QueryConstraints<Request>().SortByDescending(c => c.RequestId);
            return queryRepository.Find(constraints);
        }
    }
}
