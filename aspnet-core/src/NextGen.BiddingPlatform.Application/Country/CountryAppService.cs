using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Authorization;
using NextGen.BiddingPlatform.Country.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Country
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Country)]
    public class CountryAppService : BiddingPlatformAppServiceBase, ICountryAppService
    {
        private readonly IRepository<Country> _countryRepository;
        public CountryAppService(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<CountryDto> Create(CreateCountryDto input)
        {
            Country output = ObjectMapper.Map<Country>(input);
            output.UniqueId = Guid.NewGuid();
            var country = await _countryRepository.InsertAsync(output);
            return ObjectMapper.Map<CountryDto>(country);
        }

        public async Task Delete(EntityDto<Guid> input)
        {
            var country = await _countryRepository.GetAll().FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (country == null)
                throw new Exception("No data found");

            await _countryRepository.DeleteAsync(country);
        }

        public async Task<ListResultDto<CountryListDto>> GetAllCountry()
        {
            var countries = await _countryRepository.GetAllListAsync();
            var result = ObjectMapper.Map<List<CountryListDto>>(countries);
            return new ListResultDto<CountryListDto>(result);
        }
        //get all countries with filter method
        public async Task<PagedResultDto<CountryListDto>> GetCountriesByFilter(GetCountryInput input)
        {
            var countries = _countryRepository
                                        .GetAll()
                                        .AsNoTracking()
                                        .WhereIf(!input.CountryName.IsNullOrWhiteSpace(), x => x.CountryName.ToLower().Contains(input.CountryName))
                                        .AsQueryable();

            var resultCount = await countries.CountAsync();

            var result = await countries.PageBy(input).ToListAsync();

            return new PagedResultDto<CountryListDto>(resultCount, ObjectMapper.Map<IReadOnlyList<CountryListDto>>(result));
        }
        public async Task<CountryDto> GetCountryById(Guid Id)
        {
            var country = await _countryRepository.GetAll().FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (country == null)
                throw new Exception("No data found");

            return ObjectMapper.Map<CountryDto>(country);
        }

        public async Task Update(CountryDto input)
        {

            var country = await _countryRepository.GetAll().FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (country == null)
                throw new Exception("No data found");

            country.CountryCode = input.CountryCode;
            country.CountryName = input.CountryName;
            await _countryRepository.UpdateAsync(country);
        }
    }
}
