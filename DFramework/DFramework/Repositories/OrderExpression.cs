namespace DFramework.Repositories
{
    using System;
    using System.Linq.Expressions;

    public class OrderExpression
    {
        public OrderExpression(string orderByField, SortOrder sortOrder = SortOrder.Unspecified)
        {
            this.OrderByField = orderByField;
            this.SortOrder = sortOrder;
        }

        public string OrderByField { get; set; }

        public SortOrder SortOrder { get; set; }
    }

    public class OrderExpression<TEntity> : OrderExpression
    {
        public OrderExpression(
            Expression<Func<TEntity, dynamic>> orderByExpression,
            SortOrder sortOrder = SortOrder.Unspecified)
            : base(null, sortOrder)
        {
            this.OrderByExpression = orderByExpression;
        }

        public Expression<Func<TEntity, dynamic>> OrderByExpression { get; set; }
    }
}