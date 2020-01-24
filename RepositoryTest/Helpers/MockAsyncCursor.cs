using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryTest.Helpers
{
    public class MockAsyncCursor<T> : IAsyncCursor<T>
    {
        private readonly IEnumerable<T> _items;
        private bool called = false;

        public MockAsyncCursor(IEnumerable<T> items)
        {
            _items = items ?? Enumerable.Empty<T>();
        }

        public IEnumerable<T> Current => _items;

        public bool MoveNext(CancellationToken cancellationToken = new CancellationToken())
        {
            return !called && (called = true);
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(MoveNext(cancellationToken));
        }

        public void Dispose()
        {
        }
    }

    //public class FakeAsyncCursor<TEntity> : IAsyncCursor<TEntity>
    //{
    //    private IEnumerable<TEntity> items;

    //    public FakeAsyncCursor(IEnumerable<TEntity> items)
    //    {
    //        this.items = items;
    //    }

    //    public IEnumerable<TEntity> Current => items;

    //    public void Dispose()
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public bool MoveNext(CancellationToken cancellationToken = default)
    //    {
    //        return false;
    //    }

    //    public Task<bool> MoveNextAsync(CancellationToken cancellationToken = default)
    //    {
    //        return Task.FromResult(false);
    //    }
    //}
}
