using System.Threading.Tasks;
using Abp.Application.Services;
using NextGen.BiddingPlatform.Editions.Dto;
using NextGen.BiddingPlatform.MultiTenancy.Dto;

namespace NextGen.BiddingPlatform.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}