using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Country.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Country
{
    public class CountryAppService : BiddingPlatformAppServiceBase, ICountryAppService
    {
        private readonly IRepository<Country> _countryRepository;
        public CountryAppService(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<CountryDto> Create(CountryDto input)
        {
            Country output = ObjectMapper.Map<Country>(input);
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

        public async Task<List<CountryDto>> GetAllCountry()
        {
            var countries = await _countryRepository.GetAllListAsync();
            return ObjectMapper.Map<List<CountryDto>>(countries);
        }

        public async Task<CountryDto> GetCountryById(EntityDto<Guid> input)
        {
            var country = await _countryRepository.GetAll().FirstOrDefaultAsync(x => x.UniqueId == input.Id);
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
