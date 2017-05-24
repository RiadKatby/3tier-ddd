using RefactorName.Core.Entities;
using RefactorName.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Domain
{
    public class InvoiceDomain
    {
        public static InvoiceDomain Obj { get; private set; }

        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;

        static InvoiceDomain()
        {
            Obj = new InvoiceDomain();
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public bool Delete(Invoice entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "must not be null.");

            return repository.Delete(entity);
        }

        public Invoice Get(int entityId)
        {
            if (entityId <= 0)
                throw new ArgumentOutOfRangeException(nameof(entityId), "must be grater than zero.");

            return queryRepository.SingleWithGraph<Invoice>(entityId);
        }
    }
}
