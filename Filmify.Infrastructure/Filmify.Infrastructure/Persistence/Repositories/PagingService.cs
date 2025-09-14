using Filmify.Application.Common.Paging;
using Filmify.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Filmify.Infrastructure.Persistence.Repositories;

public class PagingService : IPagingService
{
    public async Task<KeysetPagingResult<T>> ToHybridPageAsync<T, TKey>(
        IQueryable<T> query,
        Expression<Func<T, TKey>> keySelector,
        KeysetPagingRequest request,
        CancellationToken cancellationToken = default)
        where TKey : IComparable<TKey>
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        // --- OFFSET Pagination if PageNumber is specified
        if (request.PageNumber.HasValue && request.PageNumber.Value > 0)
        {
            int skip = (request.PageNumber.Value - 1) * request.PageSize;
            query = request.Direction == Application.Common.Sorting.SortDirection.Asc
                ? query.OrderBy(keySelector)
                : query.OrderByDescending(keySelector);

            var list = await query.Skip(skip).Take(request.PageSize + 1).ToListAsync(cancellationToken);
            bool hasNext = list.Count > request.PageSize;
            if (hasNext) list.RemoveAt(list.Count - 1);

            return new KeysetPagingResult<T>(list.AsReadOnly(), hasNext);
        }

        // --- Keyset Pagination if LastKey is specified
        if (!string.IsNullOrWhiteSpace(request.LastKey))
        {
            TKey lastKeyValue = ParseKey<TKey>(request.LastKey);

            var parameter = keySelector.Parameters[0];
            Expression member = keySelector.Body;

            if (member.NodeType == ExpressionType.Convert || member.NodeType == ExpressionType.ConvertChecked)
                member = ((UnaryExpression)member).Operand;

            var constant = Expression.Constant(lastKeyValue, member.Type);
            Expression comparison = request.Direction == Application.Common.Sorting.SortDirection.Asc
                ? Expression.GreaterThan(member, constant)
                : Expression.LessThan(member, constant);

            var predicate = Expression.Lambda<Func<T, bool>>(comparison, parameter);
            query = query.Where(predicate);
        }

        // --- Apply sorting
        query = request.Direction == Application.Common.Sorting.SortDirection.Asc
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);

        var keysetList = await query.Take(request.PageSize + 1).ToListAsync(cancellationToken);
        bool hasNextPage = keysetList.Count > request.PageSize;
        if (hasNextPage) keysetList.RemoveAt(keysetList.Count - 1);

        return new KeysetPagingResult<T>(keysetList.AsReadOnly(), hasNextPage);
    }

    private static TKey ParseKey<TKey>(string keyStr)
    {
        var t = typeof(TKey);
        try
        {
            if (t == typeof(Guid))
                return (TKey)(object)Guid.Parse(keyStr);
            if (t.IsEnum)
                return (TKey)Enum.Parse(t, keyStr, true);
            if (t == typeof(DateTime))
                return (TKey)(object)DateTime.Parse(keyStr);
            if (t == typeof(DateTimeOffset))
                return (TKey)(object)DateTimeOffset.Parse(keyStr);
            return (TKey)Convert.ChangeType(keyStr, t);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Cannot convert LastKey '{keyStr}' to {t.Name}.", ex);
        }
    }
}

