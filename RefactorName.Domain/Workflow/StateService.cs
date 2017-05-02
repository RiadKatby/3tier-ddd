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
    public class StateService
    {
        //public static
        public static StateService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static StateService()
        {
            Obj = new StateService();
        }

        private StateService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public Dictionary<string, string> GetAllStatesDictionary(int processId)
        {
            var result = new Dictionary<string, string>();

            var constraints = new QueryConstraints<State>()
                .Page(1, int.MaxValue)
                .Where(c => true)
                .AndAlso(s=>s.ProcessId==processId);

            foreach (State state in queryRepository.Find(constraints).Items.ToList())
            {
                result.Add(state.StateId.ToString(), state.Name);
            }

            return result;
        }

        //public State Create(State entity)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException("State", "must not be null.");

        //    if (entity.Validate() == false)
        //        throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

        //    var tempEntity = repository.Create(entity);

        //    if (tempEntity != null)
        //        return tempEntity;

        //    return null;
        //}

        public IQueryResult<State> Find(StateSearchCriteria StateSearchCriteria)
        {
            if (StateSearchCriteria == null)
                throw new ArgumentNullException("StateSearchCriteria", "must not be null.");

            var constraints = new QueryConstraints<State>().IncludePath(s => s.StateType).IncludePath(s=>s.Process).SortByDescending(s => s.ProcessId).SortByDescending(s=>s.StateTypeId)

                .Page(StateSearchCriteria.PageNumber, StateSearchCriteria.PageSize).Where(s => s.StateId > 0);


            if (!string.IsNullOrEmpty(StateSearchCriteria.Name))
                constraints.AndAlso(s => s.Name.Contains(StateSearchCriteria.Name));

            if (!string.IsNullOrEmpty(StateSearchCriteria.Description))
                constraints.AndAlso(s => s.Description.Contains(StateSearchCriteria.Description));

            if (StateSearchCriteria.ProcessId != null && StateSearchCriteria.ProcessId > 0)
                constraints.AndAlso(s => s.ProcessId == StateSearchCriteria.ProcessId);

            if (StateSearchCriteria.StateTypeId != null && StateSearchCriteria.StateTypeId > 0)
                constraints.AndAlso(s => s.StateTypeId == StateSearchCriteria.StateTypeId);

            return queryRepository.Find(constraints);
        }

        public State FindById(int StateID)
        {
            if (StateID == 0)
                throw new ArgumentNullException("StateID", "must not be null.");

            var constraints = new QueryConstraints<State>()
                .Where(s => s.StateId == StateID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<State> FindByProcessId(int ProcessId)
        {
            if (ProcessId < 0)
                throw new ArgumentNullException("ProcessId", "must not be null.");

            var constraints = new QueryConstraints<State>()
                .Where(s => s.ProcessId == ProcessId);

            return queryRepository.Find(constraints);
        }


        public IQueryResult<State> GetAllStatees()
        {
            var constraints = new QueryConstraints<State>().IncludePath(s=>s.StateType).SortByDescending(c => c.StateId);
            return queryRepository.Find(constraints);
        }

        public State Update(State entity)
        {
            if (entity == null)
                throw new ArgumentNullException("State", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            var tempEntity = repository.Update<State>(entity);

            if (tempEntity != null)
                return tempEntity;

            return null;
        }
        public bool Delete(State entity)
        {
            if (entity == null)
                throw new ArgumentNullException("State", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            return repository.Delete<State>(entity);
        }
    }
}
