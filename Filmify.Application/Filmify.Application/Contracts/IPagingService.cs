using Filmify.Application.Common.Paging;
using System.Linq.Expressions;

namespace Filmify.Application.Contracts;

public interface IPagingService
{
    Task<KeysetPagingResult<T, TKey>> ToHybridPageAsync<T, TKey>(
        IQueryable<T> query,
        Expression<Func<T, TKey>> keySelector,
        KeysetPagingRequest request,
        CancellationToken cancellationToken = default)
        where TKey : IComparable<TKey>;
}
