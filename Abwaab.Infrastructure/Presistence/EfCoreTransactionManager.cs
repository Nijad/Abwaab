using Abwaab.Application.Interfaces;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Abwaab.Infrastructure.Presistence
{
    public class EfCoreTransactionManager : ITransactionManager
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public EfCoreTransactionManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
                throw new InvalidOperationException("A transaction is already in progress.");

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No transaction in progress.");

            await _currentTransaction.CommitAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                return;

            await _currentTransaction.RollbackAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}
