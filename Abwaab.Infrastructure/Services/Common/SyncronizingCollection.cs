namespace Abwaab.Infrastructure.Services.Common
{
    // C#
    public static class SyncronizingCollection
    {
        public static async Task Sync<TExisting, TDto>(
            ICollection<TExisting> existingItems,
            ICollection<TDto> incomingItems,
            Func<TExisting, TDto, bool> matchPredicate,
            Func<TExisting, Task> deleteAction,
            Func<TExisting, TDto, Task> updateAction,
            Func<TDto, Task> addAction)
            where TExisting : class
        {
            // Mark missing items for deletion
            var itemsToRemove = existingItems
                .Where(e => !incomingItems.Any(d => matchPredicate(e, d)))
                .ToList();

            foreach (var item in itemsToRemove)
                await deleteAction(item).ConfigureAwait(false);

            // Update or add items
            foreach (var dto in incomingItems)
            {
                var existing = existingItems.FirstOrDefault(e => matchPredicate(e, dto));
                if (existing != null)
                    await updateAction(existing, dto).ConfigureAwait(false); // Update existing
                else
                    await addAction(dto).ConfigureAwait(false); // Add new
            }
        }
    }
}
