namespace Congrats.Application.Helpers.Helpers.QueryableExtensions
{
    static class QueryableExtension
    {
        public static IOrderedQueryable<T> AppendOrderByAscending<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> sortSelector)
        {
            return (source as IOrderedQueryable<T>)?.ThenBy(sortSelector)
                ?? source.OrderBy(sortSelector);
        }

        public static IOrderedQueryable<T> AppendOrderByDescending<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> sortSelector)
        {
            return (source as IOrderedQueryable<T>)?.ThenByDescending(sortSelector)
                ?? source.OrderByDescending(sortSelector);
        }
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, Dictionary<Expression<Func<T, bool>>, bool> filters, int pageNumber, int pageSize, ref ResponseDTO responseDto)
        {
            foreach (var filter in filters)
            {
                if (filter.Value)
                {
                    query = query.Where(filter.Key);
                }
            }
            if (pageNumber == 0)
                pageNumber = 1;

            if (pageSize == 0)
            {
                query = query.AsQueryable();
                responseDto.StatusEnum = StatusEnum.Success;
                responseDto.PageIndex = pageNumber;
                responseDto.PageSize = query.Count();
                responseDto.TotalItems = query.Count();
                responseDto.TotalPages = pageNumber;

                return query;
            }

            query = query
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
            var count = query.Count();

            responseDto.PageIndex = pageNumber;
            responseDto.PageSize = pageSize;
            responseDto.TotalItems = count;
            responseDto.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            responseDto.StatusEnum = StatusEnum.Success;

            return query;
        }


    }
}
