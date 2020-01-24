using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryTest.Helpers
{
    public class FakeFindFluent<TEntity, TProjection> : IFindFluent<TEntity, TEntity>
    {
        private readonly IEnumerable<TEntity> _items;

        public FakeFindFluent(IEnumerable<TEntity> items)
        {
            _items = items ?? Enumerable.Empty<TEntity>();
        }

        public FilterDefinition<TEntity> Filter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public FindOptions<TEntity, TEntity> Options => throw new NotImplementedException();

        public IFindFluent<TEntity, TResult> As<TResult>(MongoDB.Bson.Serialization.IBsonSerializer<TResult> resultSerializer = null)
        {
            throw new NotImplementedException();
        }

        public long Count(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public long CountDocuments(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountDocumentsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IFindFluent<TEntity, TEntity> Limit(int? limit)
        {
            throw new NotImplementedException();
        }

        public IFindFluent<TEntity, TNewProjection> Project<TNewProjection>(ProjectionDefinition<TEntity, TNewProjection> projection)
        {
            throw new NotImplementedException();
        }

        public IFindFluent<TEntity, TEntity> Skip(int? skip)
        {
            throw new NotImplementedException();
        }

        public IFindFluent<TEntity, TEntity> Sort(SortDefinition<TEntity> sort)
        {
            throw new NotImplementedException();
        }

        public IAsyncCursor<TEntity> ToCursor(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncCursor<TEntity>> ToCursorAsync(CancellationToken cancellationToken = default)
        {
            IAsyncCursor<TEntity> cursor = new MockAsyncCursor<TEntity>(_items);
            var task = Task.FromResult(cursor);

            return task;
        }
    }
}
