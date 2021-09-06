using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Auction.Dto;
using NextGen.BiddingPlatform.Core.AppAccountEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.Authorization;
using NextGen.BiddingPlatform.Authorization;
using Abp.UI;
using NextGen.BiddingPlatform.Authorization.Users;

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
        private readonly IRepository<Core.AuctionItems.AuctionItem> _auctionItemRepository;
        private readonly IUserAppService _userAppService;

        public AuctionAppService(IRepository<Core.Auctions.Auction> auctionRepository,
                                  IRepository<Core.AppAccounts.AppAccount> appAccountRepository,
                                  IRepository<Event> eventRepository,
                                  IRepository<Country.Country> countryRepository,
                                  IRepository<Core.State.State> stateRepository,
                                  IRepository<Core.AuctionItems.AuctionItem> auctionItemRepository,
                                  IUserAppService userAppService)
        {
            _auctionRepository = auctionRepository;
            _appAccountRepository = appAccountRepository;
            _eventRepository = eventRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _auctionItemRepository = auctionItemRepository;
            _userAppService = userAppService;
        }

        public async Task<PagedResultDto<AuctionListDto>> GetAllAuctionFilter(AuctionTypeFilter input)
        {
            var currUser = _userAppService.GetCurrUser();
            var query = _auctionRepository.GetAllIncluding(x => x.Event, x => x.AppAccount);
            if (currUser.AppAccountId.HasValue)
                query = query.Where(x => x.AppAccountId == currUser.AppAccountId.Value);

            var result = query.WhereIf(!input.AuctionType.IsNullOrWhiteSpace(), x => x.AuctionType.ToLower().IndexOf(input.AuctionType.ToLower()) > -1)
                                          .Select(x => new AuctionListDto
                                          {
                                              UniqueId = x.UniqueId,
                                              AccountUniqueId = x.AppAccount.UniqueId,
                                              EventUniqueId = x.Event.UniqueId,
                                              AuctionEndDateTime = x.AuctionEndDateTime,
                                              AuctionStartDateTime = x.AuctionStartDateTime,
                                              AuctionType = x.AuctionType
                                          });

            var resultCount = await result.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                result = result.OrderBy(input.Sorting);

            result = result.PageBy(input);
            var resultQuery = result.ToList();
            return new PagedResultDto<AuctionListDto>(resultCount, resultQuery);
        }

        public async Task<ListResultDto<AuctionListDto>> GetAll()
        {
            var currUser = _userAppService.GetCurrUser();
            List<Core.Auctions.Auction> auctions;
            if (currUser.AppAccountId.HasValue)
                auctions = await _auctionRepository.GetAllIncluding(x => x.Event, x => x.AppAccount).Where(x => x.AppAccountId == currUser.AppAccountId.Value).ToListAsync();
            else
                auctions = await _auctionRepository.GetAllIncluding(x => x.Event, x => x.AppAccount).ToListAsync();

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

            var auctionItems = _auctionItemRepository.GetAll()
                                                     .Where(x => x.AuctionId == existingAuction.Id)
                                                     .Select(x => x.ItemId).ToList();

            var mappedData = ObjectMapper.Map<UpdateAuctionDto>(existingAuction);

            mappedData.Items = auctionItems;
            return mappedData;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Auction_Create)]
        public async Task<CreateAuctionDto> CreateAuction(CreateAuctionDto input)
        {
            var account = await _appAccountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AccountUniqueId);
            if (account == null)
                throw new UserFriendlyException("AppAccount not found for given id");

            var existingEvent = await _eventRepository.FirstOrDefaultAsync(x => x.UniqueId == input.EventUniqueId);
            if (existingEvent == null)
                throw new UserFriendlyException("Event not found for given id");

            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            if (country == null)
                throw new UserFriendlyException("Country not found for given id");

            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (state == null)
                throw new UserFriendlyException("State not found for given id");

            if (!AbpSession.TenantId.HasValue)
                throw new UserFriendlyException("You are not authorized user");

            if (!(input.AuctionStartDateTime >= existingEvent.EventStartDateTime && input.AuctionEndDateTime <= existingEvent.EventEndDateTime))
            {
                throw new UserFriendlyException("Please make sure auction start/end time between event start and end time");
            }

            var uniqueId = Guid.NewGuid();
            var auction = ObjectMapper.Map<Core.Auctions.Auction>(input);
            auction.TenantId = AbpSession.TenantId.Value;
            auction.UniqueId = uniqueId;
            auction.AuctionLink = uniqueId;
            auction.AppAccountId = account.Id;
            auction.EventId = existingEvent.Id;
            auction.Address.UniqueId = Guid.NewGuid();
            auction.Address.StateId = state.Id;
            auction.Address.CountryId = country.Id;
            auction.EventId = existingEvent.Id;
            auction.AppAccountId = account.Id;
            foreach (var item in input.Items)
            {
                auction.AuctionItems.Add(new Core.AuctionItems.AuctionItem
                {
                    ItemId = item,
                    IsActive = true,
                    UniqueId = Guid.NewGuid()
                });
            }

            await _auctionRepository.InsertAsync(auction);
            return input;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Auction_Edit)]
        public async Task<UpdateAuctionDto> UpdateAuction(UpdateAuctionDto input)
        {
            var existingEvent = await _eventRepository.FirstOrDefaultAsync(x => x.UniqueId == input.EventUniqueId);
            if (existingEvent == null)
                throw new UserFriendlyException("Event not found for given id");

            var existingAccount = await _appAccountRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AccountUniqueId);
            if (existingAccount == null)
                throw new UserFriendlyException("Account not found for given id");

            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            if (country == null)
                throw new UserFriendlyException("Country not found for given id");

            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (state == null)
                throw new UserFriendlyException("State not found for given id");

            var exisingAuction = await _auctionRepository
                                          .GetAllIncluding(x => x.Address, x => x.Address.State, x => x.Address.Country, x => x.AuctionItems)
                                          .FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);

            if (exisingAuction == null)
                throw new UserFriendlyException("Auction not found for given Id");


            if (!(input.AuctionStartDateTime >= existingEvent.EventStartDateTime && input.AuctionEndDateTime <= existingEvent.EventEndDateTime))
            {
                throw new UserFriendlyException("Please make sure auction start/end time between event start and end time");
            }


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
            exisingAuction.AuctionLink = exisingAuction.UniqueId;
            await _auctionRepository.UpdateAsync(exisingAuction);
            return input;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_Auction_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Id);
            if (auction == null)
                throw new UserFriendlyException("Auction not found for given Id");

            await _auctionRepository.DeleteAsync(auction);
        }

        public async Task<List<AuctionSelectDto>> GetAuctions()
        {
            var currUser = _userAppService.GetCurrUser();
            if (currUser.AppAccountId.HasValue)
                return await _auctionRepository.GetAllIncluding().Where(x => x.AppAccountId == currUser.AppAccountId.Value)
                                            .Select(x => new AuctionSelectDto
                                            {
                                                UniqueId = x.UniqueId,
                                                AuctionType = x.AuctionType,
                                                Id = x.Id
                                            }).ToListAsync();
            else
                return await _auctionRepository.GetAllIncluding()
                                            .Select(x => new AuctionSelectDto
                                            {
                                                UniqueId = x.UniqueId,
                                                AuctionType = x.AuctionType,
                                                Id = x.Id
                                            }).ToListAsync();
        }
    }
}
