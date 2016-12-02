namespace NGridSample.Features.Home
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using Domain;
    using Infrastructure;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using NGrid.Core;

    public class FetchData
    {


        public class FetchDataHandlerSampleItem : DbContextHandler<SampleItem>
        {
            public FetchDataHandlerSampleItem(ApiContext context) : base(context)
            {
            }

        }

       }
}
