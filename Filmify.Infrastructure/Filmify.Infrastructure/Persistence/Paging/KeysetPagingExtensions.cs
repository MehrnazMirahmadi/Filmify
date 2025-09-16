using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Filmify.Infrastructure.Persistence.Paging
{
    public static class KeysetPagingExtensions
    {
        public static async Task<KeysetPagingResult<T, TKey>> ToHybridPageAsync<T, TKey>(
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

                TKey lastKey = hasNext ? keySelector.Compile().Invoke(list.Last()) : default!;
                return new KeysetPagingResult<T, TKey>(list.AsReadOnly(), hasNext, lastKey);
            }

            // --- Keyset Pagination if LastKey is specified
            if (!string.IsNullOrWhiteSpace(request.LastKey))
            {
               // TKey lastKeyValue = (TKey)Convert.ChangeType(request.LastKey.Value, typeof(TKey));

                var parameter = keySelector.Parameters[0];
                Expression member = keySelector.Body;


                if (member.NodeType == ExpressionType.Convert || member.NodeType == ExpressionType.ConvertChecked)
                    member = ((UnaryExpression)member).Operand;

                // تبدیل LastKey به نوع واقعی ستون
                var memberType = member.Type;
                object lastKeyValue = Convert.ChangeType(request.LastKey, memberType);

                var constant = Expression.Constant(lastKeyValue, memberType);

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

            var lastKeyForResult = keysetList.Any() ? keySelector.Compile().Invoke(keysetList.Last()) : default!;

            return new KeysetPagingResult<T, TKey>(keysetList.AsReadOnly(), hasNextPage, lastKeyForResult);
        }

      
    }
}
