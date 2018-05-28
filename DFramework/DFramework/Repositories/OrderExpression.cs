using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DFramework.Repositories
{
    public class OrderExpression
    {
        public OrderExpression(string orderByField, SortOrder sortOrder = SortOrder.Unspecified)
        {
            OrderByField = orderByField;
            SortOrder = sortOrder;
        }

        public string OrderByField { get; set; }
        public SortOrder SortOrder { get; set; }
    }

    public class OrderExpression<TEntity> : OrderExpression
    {
        public OrderExpression(Expression<Func<TEntity, dynamic>> orderByExpression,
            SortOrder sortOrder = SortOrder.Unspecified)
            : base(null, sortOrder)
        {
            OrderByExpression = orderByExpression;
        }

        public Expression<Func<TEntity, dynamic>> OrderByExpression { get; set; }
    }
}