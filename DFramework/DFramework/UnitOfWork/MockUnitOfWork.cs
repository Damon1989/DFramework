using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace DFramework.UnitOfWork
{
    public class MockUnitOfWork : IUnitOfWork
    {
        public void Commit(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted,
                           TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
        }

        public Task CommitAsync(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted,
                                TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            return Task.FromResult<object>(null);
        }

        public Task CommitAsync(CancellationToken cancellationToken, System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted, TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            return Task.FromResult<object>(null);
        }

        public void Rollback()
        {
        }

        public void Dispose()
        {
        }
    }
}