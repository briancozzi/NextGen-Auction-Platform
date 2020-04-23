using Abp.Application.Services;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.Country.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Country
{
    public interface ICountryAppService : IApplicationService
    {
        Task<ListResultDto<CountryListDto>> GetAllCountry();
        //Task<PagedResultDto<CountryListDto>> GetAllCountry(GetCountryInput input);
        Task<CountryDto> Create(CreateCountryDto input);
        Task Update(CountryDto input);
        Task Delete(EntityDto<Guid> input);
        Task<CountryDto> GetCountryById(Guid input);
    }
}
