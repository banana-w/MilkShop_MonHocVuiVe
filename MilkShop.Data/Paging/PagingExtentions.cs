using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MilkShop.Data.Paging
{
    public static class PaginateExtension
    {
        public static async Task<Paginate<T>> ToPaginateAsync<T>(this IQueryable<T> queryable, int page, int size, int firstPage = 1)
        {
            if (firstPage > page)
                throw new ArgumentException($"page ({page}) must greater or equal than firstPage ({firstPage})");
            var total = await queryable.CountAsync();
            var items = await queryable.Skip((page - firstPage) * size).Take(size).ToListAsync();
            var totalPages = (int)Math.Ceiling(total / (double)size);
            return new Paginate<T>
            {
                Page = page,
                Size = size,
                Total = total,
                Items = items,
                TotalPages = totalPages
            };
        }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1,
            Expression<Func<T, bool>> expression2)
        {
            InvocationExpression invocationExpression =
                Expression.Invoke((Expression)expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(
                (Expression)Expression.AndAlso(expression1.Body, (Expression)invocationExpression),
                (IEnumerable<ParameterExpression>)expression1.Parameters);
        }
    }
}
