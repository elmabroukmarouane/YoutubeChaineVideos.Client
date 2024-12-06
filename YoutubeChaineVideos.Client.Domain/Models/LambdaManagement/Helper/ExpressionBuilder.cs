using YoutubeChaineVideos.Client.Domain.Models.LambdaManagement.Models;
using System.Linq.Expressions;

namespace YoutubeChaineVideos.Client.Domain.Models.LambdaManagement.Helper
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<T, bool>> BuildLambda<T>(LambdaExpressionModel model)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            // Safeguard against null or invalid groups
            if (model.RootGroup == null)
            {
                throw new ArgumentNullException(nameof(model.RootGroup), "RootGroup cannot be null.");
            }

            var body = BuildGroupExpression<T>(model.RootGroup, parameter);

            // Default to 'true' if the body is invalid or null
            if (body == null || body.Type != typeof(bool))
            {
                body = Expression.Constant(true);
            }

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression BuildGroupExpression<T>(ConditionGroupModel group, ParameterExpression parameter)
        {
            Expression? combined = null;

            if (group.Conditions != null && group.Conditions.Any())
            {
                foreach (var condition in group.Conditions)
                {
                    var property = Expression.Property(parameter, condition.PropertyName);
                    var propertyType = property.Type;

                    // Handle nullable types by getting the underlying type
                    var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

                    // Create the appropriate comparison expression
                    Expression comparison = condition.ComparisonType switch
                    {
                        // Boolean-specific comparisons
                        "IsTrue" when underlyingType == typeof(bool) =>
                            Expression.Equal(property, Expression.Constant(true, propertyType)),
                        "IsFalse" when underlyingType == typeof(bool) =>
                            Expression.Equal(property, Expression.Constant(false, propertyType)),

                        // Null checks
                        "IsNull" => Expression.Equal(property, Expression.Constant(null, propertyType)),
                        "IsNotNull" => Expression.NotEqual(property, Expression.Constant(null, propertyType)),

                        // Numeric, DateTime, or string-specific comparisons
                        "Equal" => Expression.Equal(property, GetComparisonValue(propertyType, condition.ComparisonValue)),
                        "NotEqual" => Expression.NotEqual(property, GetComparisonValue(propertyType, condition.ComparisonValue)),
                        "GreaterThan" => Expression.GreaterThan(property, GetComparisonValue(propertyType, condition.ComparisonValue)),
                        "GreaterThanOrEqual" => Expression.GreaterThanOrEqual(property, GetComparisonValue(propertyType, condition.ComparisonValue)),
                        "LessThan" => Expression.LessThan(property, GetComparisonValue(propertyType, condition.ComparisonValue)),
                        "LessThanOrEqual" => Expression.LessThanOrEqual(property, GetComparisonValue(propertyType, condition.ComparisonValue)),

                        // String-specific comparisons
                        "Contains" when underlyingType == typeof(string) =>
                            Expression.Call(property, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, GetComparisonValue(propertyType, condition.ComparisonValue)),
                        "NotContains" when underlyingType == typeof(string) =>
                            Expression.Not(Expression.Call(property, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, GetComparisonValue(propertyType, condition.ComparisonValue))),
                        "StartsWith" when underlyingType == typeof(string) =>
                            Expression.Call(property, typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!, GetComparisonValue(propertyType, condition.ComparisonValue)),
                        "EndsWith" when underlyingType == typeof(string) =>
                            Expression.Call(property, typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!, GetComparisonValue(propertyType, condition.ComparisonValue)),

                        // Unsupported comparison type
                        _ => throw new NotSupportedException($"Comparison type '{condition.ComparisonType}' is not supported for property '{condition.PropertyName}'.")
                    };

                    // Combine conditions using the logical operator
                    combined = combined == null ? comparison : CombineExpressions(combined, comparison, group.LogicalOperator);
                }
            }

            if (group.Groups != null && group.Groups.Any())
            {
                foreach (var subgroup in group.Groups)
                {
                    var subgroupExpression = BuildGroupExpression<T>(subgroup, parameter);
                    combined = combined == null ? subgroupExpression : CombineExpressions(combined, subgroupExpression, group.LogicalOperator);
                }
            }

            return combined ?? Expression.Constant(true);
        }

        // Helper method to get comparison value while handling null
        private static Expression GetComparisonValue(Type propertyType, object? comparisonValue)
        {
            // Avoid conversion for null
            if (comparisonValue == null)
                return Expression.Constant(null, propertyType);

            // Convert the value to the correct type
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            var convertedValue = Convert.ChangeType(comparisonValue, underlyingType);
            return Expression.Constant(convertedValue, propertyType);
        }


        private static Expression CombineExpressions(Expression left, Expression right, string logicalOperator) => logicalOperator switch
        {
            "AND" => Expression.AndAlso(left, right),
            "OR" => Expression.OrElse(left, right),
            _ => throw new NotSupportedException($"Logical operator '{logicalOperator}' is not supported.")
        };
    }
}
