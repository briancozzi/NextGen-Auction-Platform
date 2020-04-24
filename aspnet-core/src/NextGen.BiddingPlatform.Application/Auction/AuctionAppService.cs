using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Auction.Dto;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Auction
{
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

        public async Task<ListResultDto<AuctionListDto>> GetAll()
        {
            var auctions = await _auctionRepository.GetAllIncluding(x => x.Address).ToListAsync();
            return new ListResultDto<AuctionListDto>(ObjectMapper.Map<IReadOnlyList<AuctionListDto>>(auctions));
        }

        public async Task<AuctionDto> GetAuctionById(Guid Id)
        {
            var existingAuction = await _auctionRepository.GetAllIncluding(x => x.Address, x => x.Event, x => x.AppAccount).FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (existingAuction == null)
                throw new Exception("Auction data not found for given id");

            return ObjectMapper.Map<AuctionDto>(existingAuction);
        }

        public async Task CreateAuction(CreateAuctionDto input)
        {
            var account = await _appAccountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AccountId);
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
            await _auctionRepository.InsertAsync(auction);
        }

        public async Task UpdateAuction(UpdateAuctionDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            if (country == null)
                throw new Exception("Country not found for given id");
            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (state == null)
                throw new Exception("State not found for given id");

            var exisingAuction = await _auctionRepository
                                          .GetAllIncluding(x => x.Address, x => x.Address.State, x => x.Address.Country)
                                          .FirstOrDefaultAsync(x => x.UniqueId == input.Id);

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
        }
    }
}
