using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace DFramework.UnitOfWork
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        public virtual void Dispose()
        {
        }

        public abstract void Commit(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required);

        public abstract Task CommitAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required);

        public abstract Task CommitAsync(CancellationToken cancellationToken,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required);

        public abstract void Rollback();
    }
}