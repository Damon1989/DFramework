using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using DFramework.UnitOfWork;
using IsolationLevel = System.Data.IsolationLevel;

namespace DFramework.EntityFramework.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected List<MSDbContext> _dbContexts;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Commit(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync(CancellationToken cancellationToken, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}