using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryTest.Helpers
{
    public class MockAsyncCursor<T> : IAsyncCursor<T>
    {
        private bool _called = false;

        public MockAsyncCursor(IEnumerable<T> items)
        {
            Current = items ?? Enumerable.Empty<T>();
        }

        public IEnumerable<T> Current { get; }

        public bool MoveNext(CancellationToken cancellationToken = new CancellationToken())
        {
            return !_called && (_called = true);
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(MoveNext(cancellationToken));
        }

        public void Dispose()
        {
        }
    }
}
