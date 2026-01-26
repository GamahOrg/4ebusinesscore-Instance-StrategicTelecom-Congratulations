namespace _4EPlatForm.Infrastructure.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> FilterDynamic<T>(this IQueryable<T> query,
            IQueryable<T> realQury, string fieldName, ICollection<string> values)
        {
            var param = Expression.Parameter(typeof(T), "e");
            // if (param.IsByRef != false) {
            var prop = Expression.PropertyOrField(param, fieldName);
            var body = Expression.Call(typeof(Enumerable), "Contains", new[] { typeof(string) },
                Expression.Constant(values), prop);
            var predicate = Expression.Lambda<Func<T, bool>>(body, param);
            return realQury.Where(predicate);
            // }
            // return query;

        }


        public static IQueryable<T> Paginage<T>(this IQueryable<T> requestInstancesQuery, int PageNumber, int PageSize, ref ResponseDTO _responseDTO)
       where T : AuditEntity<Guid>
        {


            requestInstancesQuery = requestInstancesQuery.OrderByDescending(a => a.CreatedOn);
            if (PageNumber <= 0)
            {
                PageNumber = 1;
            }

            if (PageSize <= 0)
            {
                PageSize = 10;
            }
            var count = requestInstancesQuery.Count();
            _responseDTO.PageIndex = PageNumber;
            _responseDTO.PageSize = PageSize;
            _responseDTO.TotalItems = count;
            _responseDTO.TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            return requestInstancesQuery.Skip((PageNumber - 1) * PageSize).Take(PageSize);

        }


        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int pageIndex, int pageSize, ref DTO.ResponseDTO responseDto)
        {

            var items = queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable();
            var count = queryable.Count();

            responseDto.PageIndex = pageIndex;
            responseDto.PageSize = pageSize;
            responseDto.TotalItems = count;
            responseDto.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            responseDto.StatusEnum = StatusEnum.Success;

            return items;
        }

    }
}
