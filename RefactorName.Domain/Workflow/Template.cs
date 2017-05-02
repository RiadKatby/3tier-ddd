//using RefactorName.Core;
//using RefactorName.Core.SearchEntities;
//using RefactorName.Core.Workflow;
//using RefactorName.RepositoryInterface;
//using RefactorName.RepositoryInterface.Queries;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RefactorName.Domain.Workflow
//{
//    public class TemplateService
//    {
//        //public static
//        public static TemplateService Obj { get; private set; }
//        private static IGenericRepository repository;
//        private static IGenericQueryRepository queryRepository;



//        static TemplateService()
//        {
//            Obj = new TemplateService();
//        }

//        private TemplateService()
//        {
//            repository = RepositoryFactory.CreateRepository();
//            queryRepository = RepositoryFactory.CreateQueryRepository();
//        }

//        public Template Create(Template entity)
//        {
//            if (entity == null)
//                throw new ArgumentNullException("Template", "must not be null.");

//            if (entity.Validate() == false)
//                throw new ValidationException("Business Entity has invalid information.", entity.ValidationResults, ErrorCode.InvalidData);

//            var tempEntity = repository.Create(entity);

//            if (tempEntity != null)
//                return tempEntity;

//            return null;
//        }

//        public IQueryResult<Template> Find(TemplateSearchCriteria TemplateSearchCriteria)
//        {
//            if (TemplateSearchCriteria == null)
//                throw new ArgumentNullException("TemplateSearchCriteria", "must not be null.");

//            var constraints = new QueryConstraints<Template>().IncludePath(p => p.Name).SortByDescending(p => p.TemplateId)

//                .Page(TemplateSearchCriteria.PageNumber, TemplateSearchCriteria.PageSize).Where(p => p.TemplateId > 0);


//            if (!string.IsNullOrEmpty(TemplateSearchCriteria.TemplateName))
//                constraints.AndAlso(p => p.Name.Contains(TemplateSearchCriteria.TemplateName));

//            return queryRepository.Find(constraints);
//        }

//        public Template FindById(int TemplateID)
//        {
//            if (TemplateID == 0)
//                throw new ArgumentNullException("TemplateID", "must not be null.");

//            var constraints = new QueryConstraints<Template>()
//                .Where(p => p.TemplateId == TemplateID);

//            return queryRepository.SingleOrDefault(constraints);
//        }

//        public IQueryResult<Template> GetAllTemplatees()
//        {
//            var constraints = new QueryConstraints<Template>().SortByDescending(c => c.TemplateId);
//            return queryRepository.Find(constraints);
//        }
//    }
//}
