using RefactorName.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.SqlServerRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IGenericRepository repository;
        private IGenericQueryRepository queryRepository;
        private MyDbContext context;

        public UnitOfWork()
        {
            context = new MyDbContext();
            repository = new GenericRepository(context);
            queryRepository = new GenericQueryRepository(context);
        }

        #region IUnitOfWork Members

        public IGenericRepository Repository
        {
            get { return repository; }
        }

        public IGenericQueryRepository QueryRepository
        {
            get { return queryRepository; }
        }

        public int Commit()
        {
            return context.SaveChanges();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            context.Dispose();
            context = null;
        }

        #endregion
    }
}
