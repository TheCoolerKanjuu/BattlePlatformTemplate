namespace Presentation.BP.Helpers;

using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using System.Linq.Expressions;

public static class DynamicLinqExtensions
{
    public static Expression<Func<TEntity, bool>>? ParseFilterExpression<TEntity>(string? filterExpression)
    {
        if (string.IsNullOrEmpty(filterExpression))
        {
            return null;
        }

        try
        {
            return DynamicExpressionParser.ParseLambda<TEntity, bool>(new ParsingConfig(), false, filterExpression);
            
        }
        catch (ParseException ex)
        {
            throw new ArgumentException($"Invalid filter expression: {ex.Message}", nameof(filterExpression), ex);
        }
    }

    public static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ParseOrderByExpression<TEntity>(string? orderByExpression)
    {
        if (string.IsNullOrEmpty(orderByExpression))
            return null;

        try
        {
            return DynamicExpressionParser.ParseLambda<IQueryable<TEntity>, IOrderedQueryable<TEntity>>(new ParsingConfig(), false, $"OrderBy({orderByExpression})").Compile();
        }
        catch (ParseException ex)
        {
            throw new ArgumentException($"Invalid order by expression: {ex.Message}", nameof(orderByExpression), ex);
        }
    }
}