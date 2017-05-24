using RefactorName.Core.Entities;
using RefactorName.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Domain
{
    public class ItemDomain
    {
        public static ItemDomain Obj { get; private set; }

        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;

        static ItemDomain()
        {
            Obj = new ItemDomain();
            repository = RepositoryFactory.CreateRepository();
            queryRepository = RepositoryFactory.CreateQueryRepository();
        }

        public Item Create(Item entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "must not be null.");

            return repository.Create<Item>(entity);
        }

        public Item Update(Item entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "must not be null.");

            return repository.Update<Item>(entity);
        }

        public Item Get(int entityId)
        {
            if (entityId <= 0)
                throw new ArgumentOutOfRangeException(nameof(entityId), "must be grater than zero.");

            return queryRepository.Single<Item>(entityId);
        }

        public bool Delete(Item entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "must not be null.");

            return repository.Delete<Item>(entity);
        }
    }
}
