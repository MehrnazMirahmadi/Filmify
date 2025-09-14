using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Filmify.Infrastructure.Persistence.Paging
{
    public static class KeysetPagingExtensions
    {
        /// <summary>
        /// Hybrid Keyset + Offset Pagination
        /// - Use PageNumber for direct page access (offset-based)
        /// - Use LastKey for next/previous page (keyset-based)
        /// </summary>
        public static async Task<KeysetPagingResult<T>> ToHybridPageAsync<T, TKey>(
            this IQueryable<T> query,
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
                query = request.Direction == SortDirection.Asc
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

                // Remove Convert/Boxing if present
                if (member.NodeType == ExpressionType.Convert || member.NodeType == ExpressionType.ConvertChecked)
                    member = ((UnaryExpression)member).Operand;

                var constant = Expression.Constant(lastKeyValue, member.Type);

                Expression comparison = request.Direction == SortDirection.Asc
                    ? Expression.GreaterThan(member, constant)
                    : Expression.LessThan(member, constant);

                var predicate = Expression.Lambda<Func<T, bool>>(comparison, parameter);
                query = query.Where(predicate);
            }

            // --- Apply sorting
            query = request.Direction == SortDirection.Asc
                ? query.OrderBy(keySelector)
                : query.OrderByDescending(keySelector);

            // --- Take one more item to check if there is a next page
            var keysetList = await query.Take(request.PageSize + 1).ToListAsync(cancellationToken);
            bool hasNextPage = keysetList.Count > request.PageSize;
            if (hasNextPage) keysetList.RemoveAt(keysetList.Count - 1);

            return new KeysetPagingResult<T>(keysetList.AsReadOnly(), hasNextPage);
        }

        /// <summary>
        /// Convert string LastKey to TKey
        /// Supports int, long, double, decimal, DateTime, Guid, Enum
        /// </summary>
        private static TKey ParseKey<TKey>(string keyStr)
        {
            var t = typeof(TKey);

            try
            {
                if (t == typeof(Guid))
                    return (TKey)(object)Guid.Parse(keyStr);

                if (t.IsEnum)
                    return (TKey)Enum.Parse(t, keyStr, ignoreCase: true);

                if (t == typeof(DateTime))
                    return (TKey)(object)DateTime.Parse(keyStr);

                if (t == typeof(DateTimeOffset))
                    return (TKey)(object)DateTimeOffset.Parse(keyStr);

                // numeric types: int, long, double, decimal, float, short, byte, etc.
                return (TKey)Convert.ChangeType(keyStr, t);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Cannot convert LastKey '{keyStr}' to type {t.Name}.", ex);
            }
        }
    }
}
