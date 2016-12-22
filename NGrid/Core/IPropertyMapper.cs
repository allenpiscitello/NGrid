namespace NGrid.Core
{
    using System;
    using System.Linq.Expressions;

    public interface IPropertyMapper<T>
    {
        Expression<Func<T, object>> GetQueryExpression();
    }

}
