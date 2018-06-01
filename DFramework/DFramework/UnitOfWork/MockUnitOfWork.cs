using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace DFramework.UnitOfWork
{
    public class MockUnitOfWork : IUnitOfWork
    {
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

        public void Dispose()
        {
        }

        public Task CommitAsync(CancellationToken cancellationToken,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            throw new NotImplementedException();
        }
    }
}