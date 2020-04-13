using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen-Auction-Platform.Roles.Dto;
using NextGen-Auction-Platform.Users.Dto;

namespace NextGen-Auction-Platform.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);
    }
}
