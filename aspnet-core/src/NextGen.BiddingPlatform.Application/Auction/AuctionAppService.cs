using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Auction.Dto;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
using NextGen.BiddingPlatform.DashboardCustomization.Dto;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using IdentityServer4.Extensions;
using Abp.Extensions;
using Abp.Authorization;
using NextGen.BiddingPlatform.Authorization;

namespace NextGen.BiddingPlatform.Auction
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Auction)]
    public class AuctionAppService : BiddingPlatformAppServiceBase, IAuctionAppService
    {
        private readonly IRepository<Core.Auctions.Auction> _auctionRepository;
        private readonly IRepository<Core.AppAccounts.AppAccount> _appAccountRepository;
        private readonly IRepository<Country.Country> _countryRepository;
        private readonly IRepository<Core.State.State> _stateRepository;
        private readonly IRepository<Event> _eventRepository;

        private new readonly IAbpSession AbpSession;
        public AuctionAppService(IRepository<Core.Auctions.Auction> auctionRepository,
                                  IAbpSession abpSession,
                                  IRepository<Core.AppAccounts.AppAccount> appAccountRepository,
                                  IRepository<Event> eventRepository,
                                  IRepository<Country.Country> countryRepository,
                                  IRepository<Core.State.State> stateRepository)
        {
            _auctionRepository = auctionRepository;
            AbpSession = abpSession;
            _appAccountRepository = appAccountRepository;
            _eventRepository = eventRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
        }

        public async Task<PagedResultDto<AuctionListDto>> GetAllAuctionFilter(AuctionTypeFilter input)
        {
            var query = _auctionRepository.GetAllIncluding(x => x.Event, x => x.AppAccount)
                                          .WhereIf(!input.AuctionType.IsNullOrWhiteSpace(), x => x.AuctionType.ToLower().IndexOf(input.AuctionType.ToLower()) > -1)
                                          .Select(x => new AuctionListDto
                                          {
                                              UniqueId = x.UniqueId,
                                              AppAccountUniqueId = x.AppAccount.UniqueId,
                                              EventUniqueId = x.Event.UniqueId,
                                              AuctionEndDateTime = x.AuctionEndDateTime,
                                              AuctionStartDateTime = x.AuctionStartDateTime,
                                              AuctionType = x.AuctionType
                                          });

            var resultCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);

            query = query.PageBy(input);
            var resultQuery = query.ToList();
            return new PagedResultDto<AuctionListDto>(resultCount, resultQuery);
        }

        public async Task<ListResultDto<AuctionListDto>> GetAll()
        {
            var auctions = await _auctionRepository.GetAllIncluding(x => x.Event, x => x.AppAccount).ToListAsync();
            return new ListResultDto<AuctionListDto>(ObjectMapper.Map<IReadOnlyList<AuctionListDto>>(auctions));
        }

        public async Task<UpdateAuctionDto> GetAuctionById(Guid Id)
        {
            var existingAuction = await _auctionRepository.GetAllIncluding(x => x.Address,
                                                                           x => x.Event,
                                                                           x => x.AppAccount,
                                                                           x => x.Address.Country,
                                                                           x => x.Address.State)
                                                          .FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (existingAuction == null)
                throw new Exception("Auction data not found for given id");

            return ObjectMapper.Map<UpdateAuctionDto>(existingAuction);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Auction_Create)]
        public async Task<CreateAuctionDto> CreateAuction(CreateAuctionDto input)
        {
            var account = await _appAccountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AccountUniqueId);
            if (account == null)
                throw new Exception("AppAccount not found for given id");
            var existingEvent = await _eventRepository.FirstOrDefaultAsync(x => x.UniqueId == input.EventUniqueId);
            if (existingEvent == null)
                throw new Exception("Event not found for given id");
            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            if (country == null)
                throw new Exception("Country not found for given id");
            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (state == null)
                throw new Exception("State not found for given id");

            if (!AbpSession.TenantId.HasValue)
                throw new Exception("You are not authorized user");

            var auction = ObjectMapper.Map<Core.Auctions.Auction>(input);
            auction.TenantId = AbpSession.TenantId.Value;
            auction.UniqueId = Guid.NewGuid();
            auction.AppAccountId = account.Id;
            auction.EventId = existingEvent.Id;
            auction.Address.UniqueId = Guid.NewGuid();
            auction.Address.StateId = state.Id;
            auction.Address.CountryId = country.Id;
            auction.EventId = existingEvent.Id;
            auction.AppAccountId = account.Id;
            await _auctionRepository.InsertAsync(auction);
            return input;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Auction_Edit)]
        public async Task<UpdateAuctionDto> UpdateAuction(UpdateAuctionDto input)
        {
            var existingEvent = await _eventRepository.FirstOrDefaultAsync(x => x.UniqueId == input.EventUniqueId);
            if (existingEvent == null)
                throw new Exception("Event not found for given id");

            var existingAccount = await _appAccountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AccountUniqueId);
            if (existingAccount == null)
                throw new Exception("Account not found for given id");

            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            if (country == null)
                throw new Exception("Country not found for given id");

            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (state == null)
                throw new Exception("State not found for given id");

            var exisingAuction = await _auctionRepository
                                          .GetAllIncluding(x => x.Address, x => x.Address.State, x => x.Address.Country)
                                          .FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);

            if (exisingAuction == null)
                throw new Exception("Auction not found for given Id");

            exisingAuction.AuctionType = input.AuctionType;
            exisingAuction.AuctionStartDateTime = input.AuctionStartDateTime;
            exisingAuction.AuctionEndDateTime = input.AuctionEndDateTime;
            //address property
            exisingAuction.Address.Address1 = input.Address.Address1;
            exisingAuction.Address.Address2 = input.Address.Address2;
            exisingAuction.Address.City = input.Address.City;
            exisingAuction.Address.ZipCode = input.Address.ZipCode;
            exisingAuction.Address.CountryId = country.Id;
            exisingAuction.Address.StateId = state.Id;
            exisingAuction.EventId = existingEvent.Id;
            exisingAuction.AppAccountId = existingAccount.Id;
            await _auctionRepository.UpdateAsync(exisingAuction);
            return input;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Auction_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (auction == null)
                throw new Exception("Auction not found for given Id");
            await _auctionRepository.DeleteAsync(auction);
        }
    }
}
