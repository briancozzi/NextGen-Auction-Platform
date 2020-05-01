﻿using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Core.State;
using NextGen.BiddingPlatform.Country;
using NextGen.BiddingPlatform.State.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Authorization;
using NextGen.BiddingPlatform.Authorization;

namespace NextGen.BiddingPlatform.State
{
    [AbpAuthorize(AppPermissions.Pages_Administration_State)]
    public class StateAppService : BiddingPlatformAppServiceBase, IStateAppService
    {
        private readonly IRepository<Core.State.State> _stateRepository;
        private readonly ICountryAppService _countryService;
        public StateAppService(IRepository<Core.State.State> stateRepository, ICountryAppService countryService)
        {
            _stateRepository = stateRepository;
            _countryService = countryService;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_State_Create)]
        public async Task<StateDto> Create(CreateStateDto input)
        {
            var country = await _countryService.GetCountryById(input.CountryUniqueId);
            var output = ObjectMapper.Map<Core.State.State>(input);

            output.UniqueId = Guid.NewGuid();
            output.CountryId = country.Id;

            var state = await _stateRepository.InsertAsync(output);
            return ObjectMapper.Map<StateDto>(state);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_State_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (state == null)
                throw new Exception("No data found");

            await _stateRepository.DeleteAsync(state);
        }

        public async Task<ListResultDto<StateListDto>> GetAllStates()
        {
            var states = await _stateRepository.GetAllIncluding(x => x.Country).ToListAsync();
            var result = ObjectMapper.Map<List<StateListDto>>(states);
            return new ListResultDto<StateListDto>(result);
        }

        public async Task<PagedResultDto<StateListDto>> GetStatesByFilter(GetStateInput input)
        {
            var query = _stateRepository.GetAllIncluding(x => x.Country)
                                                .AsNoTracking()
                                               .WhereIf(!input.StateCode.IsNullOrWhiteSpace(), x => x.StateCode.ToLower().IndexOf(input.StateCode.ToLower()) > -1)
                                               .Select(x => new StateListDto
                                               {
                                                   CountryName = x.Country.CountryName,
                                                   CountryUniqueId = x.Country.UniqueId,
                                                   StateCode = x.StateCode,
                                                   StateName = x.StateName,
                                                   UniqueId = x.UniqueId
                                               });

            var resultCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);

            query = query.PageBy(input);

            return new PagedResultDto<StateListDto>(resultCount, query.ToList());

        }

        public async Task<StateDto> GetStateById(Guid Id)
        {
            var state = await _stateRepository.GetAllIncluding(x => x.Country).FirstOrDefaultAsync(x => x.UniqueId == Id);

            if (state == null)
                throw new Exception("State not found for given id");

            return ObjectMapper.Map<StateDto>(state);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_State_Edit)]
        public async Task<UpdateStateDto> Update(UpdateStateDto input)
        {

            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (state == null)
                throw new Exception("State not available for given id");

            var country = await _countryService.GetCountryById(input.CountryUniqueId);

            state.StateCode = input.StateCode;
            state.StateName = input.StateName;
            state.CountryId = country.Id;
            await _stateRepository.UpdateAsync(state);
            return input;
        }
    }
}
