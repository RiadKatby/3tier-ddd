using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.RepositoryInterface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository Repository { get; }
        IGenericQueryRepository QueryRepository { get; }
        void Complete();     
    }
}
