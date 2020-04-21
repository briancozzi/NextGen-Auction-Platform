using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Core.State;
using NextGen.BiddingPlatform.Country;
using NextGen.BiddingPlatform.State.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.State
{
    public class StateAppService : BiddingPlatformDomainServiceBase, IStateAppService
    {
        private readonly IRepository<Core.State.State> _stateRepository;
        private readonly ICountryAppService _countryService;
        public StateAppService(IRepository<Core.State.State> stateRepository, ICountryAppService countryService)
        {
            _stateRepository = stateRepository;
            _countryService = countryService;
        }

        public async Task<StateDto> Create(CreateStateDto input)
        {
            var country = await _countryService.GetCountryById(input.CountryUniqueId);
            Core.State.State output = ObjectMapper.Map<Core.State.State>(input);
            output.UniqueId = Guid.NewGuid();
            output.CountryId = country.Id;
            var state = await _stateRepository.InsertAsync(output);
            return ObjectMapper.Map<StateDto>(state);
        }

        public async Task Delete(EntityDto<Guid> input)
        {
            var state = await _stateRepository.GetAll().FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (state == null)
                throw new Exception("No data found");

            await _stateRepository.DeleteAsync(state);
        }

        public async Task<List<StateListDto>> GetAllStates()
        {
            var states = await _stateRepository.GetAllIncluding(x => x.Country).ToListAsync();
            return ObjectMapper.Map<List<StateListDto>>(states);
        }

        public async Task<StateDto> GetStateById(Guid Id)
        {
            var state = await _stateRepository.GetAllIncluding(x=>x.Country).FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (state == null)
                throw new Exception("No data found");

            return ObjectMapper.Map<StateDto>(state);
        }

        public async Task Update(StateDto input)
        {

            var state = await _stateRepository.GetAll().FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (state == null)
                throw new Exception("No data found");

            var country = await _countryService.GetCountryById(input.CountryUniqueId);

            state.StateCode = input.StateCode;
            state.StateName = input.StateName;
            state.CountryId = country.Id;
            await _stateRepository.UpdateAsync(state);
        }
    }
}
