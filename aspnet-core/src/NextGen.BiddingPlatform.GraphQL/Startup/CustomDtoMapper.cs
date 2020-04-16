using AutoMapper;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.Startup
{
    public static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<User, UserDto>()
                .ForMember(dto => dto.Roles, options => options.Ignore())
                .ForMember(dto => dto.OrganizationUnits, options => options.Ignore());
        }
    }
}