namespace NGridSample.Features.Home
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using AutoMapper;
    using Domain;
    using NGrid.Core;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var map =
                CreateMap<SampleItem, SampleItemViewModel>().ApplyGridMap();
        }

      
    }
}
