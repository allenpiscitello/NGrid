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


        public class FetchDataHandlerSampleItem : DbContextHandler<SampleItem, SampleItemViewModel>
        {
            public FetchDataHandlerSampleItem(ApiContext context) : base(context)
            {
            }

            protected override Expression<Func<SampleItem, object>> GetPropertySelector(string propertyName)
            {
                if (propertyName == "ChildEntityName")
                {
                    Expression<Func<SampleItem, object>> exp = x => x.ChildEntity.Name;
                    return exp;
                }
                return base.GetPropertySelector(propertyName);

            }

            protected override IQueryable<SampleItem> Dataset {
                get { return base.DbSet.Include(x => x.ChildEntity); }
            }
    }

       }
}
