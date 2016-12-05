namespace NGridSample.Features.Home
{
    using Domain;
    using Infrastructure;
    using NGrid.Core;

    public class FetchData
    {


        public class FetchDataHandlerSampleItem : DbContextHandler<SampleItem, SampleItemViewModel>
        {
            public FetchDataHandlerSampleItem(ApiContext context) : base(context)
            {
            }

        }

       }
}
