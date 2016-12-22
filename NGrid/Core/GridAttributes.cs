namespace NGrid.Core
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class GridAttributes
    {
        public class HiddenAttribute : Attribute
        {

        }

        public class PropertyMappingAttribute : Attribute
        {
            private readonly Type _type;

            public PropertyMappingAttribute(Type type)
            {
                _type = type;
            }

            public Expression<Func<T, object>> GetExpression<T>()
            {
                var t = _type.GetConstructor(new Type[] {}).Invoke(new object[] {}) as IPropertyMapper<T>;
                return t.GetQueryExpression();
            }
        }

    }
}
