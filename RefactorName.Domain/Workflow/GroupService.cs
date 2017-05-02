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
    public class GroupService
    {
        //public static
        public static GroupService Obj { get; private set; }
        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;



        static GroupService()
        {
            Obj = new GroupService();
        }

        private GroupService()
        {
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public Dictionary<string, string> GetAllGroupsDictionary(int processId)
        {
            var result = new Dictionary<string, string>();

            var constraints = new QueryConstraints<Group>()
                .Page(1, int.MaxValue)
                .Where(c => true)
                .AndAlso(s => s.ProcessId == processId);

            foreach (Group group in queryRepository.Find(constraints).Items.ToList())
            {
                result.Add(group.GroupId.ToString(), group.Name);
            }

            return result;
        }

        //public Group Create(Group entity)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException("Group", "must not be null.");

        //    if (entity.Validate() == false)
        //        throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

        //    var tempEntity = repository.Create(entity);

        //    if (tempEntity != null)
        //        return tempEntity;

        //    return null;
        //}

        public IQueryResult<Group> Find(GroupSearchCriteria GroupSearchCriteria)
        {
            if (GroupSearchCriteria == null)
                throw new ArgumentNullException("GroupSearchCriteria", "must not be null.");

            var constraints = new QueryConstraints<Group>()
                .IncludePath(p=>p.Process)
                .SortByDescending(p => p.GroupId)

                .Page(GroupSearchCriteria.PageNumber, GroupSearchCriteria.PageSize).Where(p => p.GroupId > 0);


            if (!string.IsNullOrEmpty(GroupSearchCriteria.Name))
                constraints.AndAlso(p => p.Name.Contains(GroupSearchCriteria.Name));

            if (GroupSearchCriteria.ProcessId!=null&& GroupSearchCriteria.ProcessId>0)
                constraints.AndAlso(p => p.ProcessId==GroupSearchCriteria.ProcessId);

            return queryRepository.Find(constraints);
        }

        public Group FindById(int GroupID)
        {
            if (GroupID == 0)
                return new Group();

            if (GroupID < 0)
                throw new ArgumentNullException("GroupID", "must not be null.");

            var constraints = new QueryConstraints<Group>()
                .Where(p => p.GroupId == GroupID);

            return queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<Group> GetAllGroups()
        {
            var constraints = new QueryConstraints<Group>().SortByDescending(c => c.GroupId);
            return queryRepository.Find(constraints);
        }

        public Group Update(Group entity)
        {
            if (entity == null)
                throw new ArgumentNullException("State", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            var tempEntity = repository.Update<Group>(entity);

            if (tempEntity != null)
                return tempEntity;

            return null;
        }
        public bool Delete(Group entity)
        {
            if (entity == null)
                throw new ArgumentNullException("State", "must not be null.");

            //if (entity.Validate() == false)
            //    throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

            return  repository.Delete<Group>(entity);
        }

        public IQueryResult<Group> FindByProcessId(int processId)
        {
            if (processId == 0)
                throw new ArgumentNullException("processId", "must not be null.");

            var constraints = new QueryConstraints<Group>()
                .IncludePath(p => p.Process)
                .Where(p => p.ProcessId == processId);

            return queryRepository.Find(constraints);
        }
    }
}
