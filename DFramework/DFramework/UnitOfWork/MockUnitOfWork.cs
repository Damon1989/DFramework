using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace DFramework.UnitOfWork
{
    public class MockUnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
        }

        public void Commit(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
                           TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
        }

        public Task CommitAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
                                TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            return Task.FromResult<object>(null);
        }

        public void Rollback()
        {
        }

        public Task CommitAsync(CancellationToken cancellationToken,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            return Task.FromResult<object>(null);
        }
    }
}