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
        private TransactionScope scope;

        public UnitOfWork()
        {

            repository = new GenericRepository();
            queryRepository = new GenericQueryRepository();
            scope = new TransactionScope();
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

        public void Complete()
        {
            if (scope == null)
                throw new InvalidOperationException("Complete method need UnitOfWork that support MultiCommit.");

            scope.Complete();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            scope.Dispose();
            scope = null;
        }

        #endregion
    }
}
