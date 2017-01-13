namespace NGridSample.Features.Home
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain;
    using Infrastructure;
    using NGrid.Core;
    using Microsoft.EntityFrameworkCore;

    public class FetchData
    {


        public class ChildEntityNameMapper : IPropertyMapper<SampleItem>
        {
            public Expression<Func<SampleItem, object>> GetQueryExpression()
            {
                Expression<Func<SampleItem, object>> exp = x => x.ChildEntity.Name;
                return exp;
            }
        }

        public class BooleanMapper : IPropertyMapper<SampleItem>
        {
            public Expression<Func<SampleItem, object>> GetQueryExpression()
            {

                Expression<Func<SampleItem, object>> exp = x => x.Column3 ? "B" : "A";
                return exp;
            }
        }


        public class FetchDataHandlerSampleItem : DbContextHandler<SampleItem, SampleItemViewModel>
        {
            public FetchDataHandlerSampleItem(ApiContext context) : base(context)
            {
            }

            protected override IQueryable<SampleItem> AddIncludes(DbSet<SampleItem> dbSet)
            {
                return dbSet.Include(x => x.ChildEntity);
            }


        }
    }
}
