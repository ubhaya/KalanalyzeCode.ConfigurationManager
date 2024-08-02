using System.Linq.Expressions;

namespace KalanalyzeCode.ConfigurationManager.Application.Helpers;

public static class Extensions
{
    public static IQueryable<TSource> OrderedByDirection<TSource, TKey>(
        this IQueryable<TSource> source,
        CustomSortDirection direction, Expression<Func<TSource, TKey>> keySelector)
    {
        if (direction == CustomSortDirection.None)
            return source;

        return direction is CustomSortDirection.Descending ? 
            source.OrderByDescending(keySelector) : 
            source.OrderBy(keySelector);
    }
}