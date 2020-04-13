using Abp.Application.Services;
using NextGen-Auction-Platform.MultiTenancy.Dto;

namespace NextGen-Auction-Platform.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

