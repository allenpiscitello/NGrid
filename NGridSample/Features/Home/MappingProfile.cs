
namespace NGridSample.Features.Home
{
    using AutoMapper;
    using Domain;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SampleItem, SampleItemViewModel>()
                .ForMember(x => x.Column3, opt => opt.MapFrom(new FetchData.BooleanMapper().GetQueryExpression()));
        }
    }
}
