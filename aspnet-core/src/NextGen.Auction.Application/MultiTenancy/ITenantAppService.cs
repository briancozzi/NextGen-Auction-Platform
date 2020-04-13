using Abp.Application.Services;
using NextGen.Auction.MultiTenancy.Dto;

namespace NextGen.Auction.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

