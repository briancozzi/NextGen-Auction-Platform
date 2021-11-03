using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.AuctionBidder.Dto;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Helpers;
using NextGen.BiddingPlatform.UserEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.AuctionBidder
{
    [AbpAuthorize]
    public class AuctionBidderAppService : BiddingPlatformAppServiceBase, IAuctionBidderAppService
    {
        private readonly IRepository<Core.AuctionBidders.AuctionBidder> _auctionBidderRepository;
        private readonly IRepository<Core.Auctions.Auction> _auctionRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<UserEvent, Guid> _userEventRepository;
        private readonly IRepository<Core.AppAccountEvents.Event> _appAccountEventRepository;
        public AuctionBidderAppService(IRepository<Core.AuctionBidders.AuctionBidder> auctionBidderRepository,
                                        IAbpSession abpSession,
                                        IRepository<Core.Auctions.Auction> auctionRepository,
                                        IRepository<UserEvent, Guid> userEventRepository,
                                        IRepository<Core.AppAccountEvents.Event> appAccountEventRepository)
        {
            _auctionBidderRepository = auctionBidderRepository;
            _auctionRepository = auctionRepository;
            _abpSession = abpSession;
            _userEventRepository = userEventRepository;
            _appAccountEventRepository = appAccountEventRepository;
        }

        public async Task CreateBidder(CreateAuctionBidderDto input)
        {
            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionId);
            if (auction == null)
                throw new Exception("Auction not found for given id");

            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user == null)
                throw new Exception("User not found for given user id");

            //if (!_abpSession.TenantId.HasValue)
            //    throw new Exception("You are not authorized user");

            await _auctionBidderRepository.InsertAsync(new Core.AuctionBidders.AuctionBidder
            {
                UniqueId = Guid.NewGuid(),
                UserId = user.Id,
                AuctionId = auction.Id,
                BidderName = input.BidderName
            });
        }

        public async Task<ApiResponse<object>> CreateBidderFromExternalApp(CreateAuctionBidderWithExternalApp input)
        {
            try
            {
                var user = await UserManager.Users.FirstOrDefaultAsync(s => s.ExternalUserId == input.ExternalUserId);
                if (user == null)
                    throw new Exception("User not found for given user id");

                var appAccountEvent = await _appAccountEventRepository.FirstOrDefaultAsync(s => s.UniqueId == input.EventId);
                if (appAccountEvent == null)
                    throw new Exception("Event not found!!");

                var userEvent = await _userEventRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(s => s.UserId == user.Id && s.EventId == appAccountEvent.Id);
                if (userEvent == null)
                    throw new UserFriendlyException("User not registered with this event");

                var auctions = await _auctionRepository.GetAll().AsNoTracking().Where(s => s.EventId == userEvent.EventId).Select(s => s.Id).ToListAsync();

                foreach (var auctionId in auctions)
                {
                    var auctionBidder = await _auctionBidderRepository.GetAll()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(s => s.AuctionId == auctionId && s.UserId == user.Id);

                    if (auctionBidder == null)
                    {
                        await _auctionBidderRepository.InsertAsync(new Core.AuctionBidders.AuctionBidder
                        {
                            UniqueId = Guid.NewGuid(),
                            UserId = user.Id,
                            AuctionId = auctionId,
                            BidderName = user.FullName
                        });
                    }
                }

                return new ApiResponse<object>
                {
                    Data = null,
                    //Message = "Successfully register bidder.",
                    //Status = true,
                    //StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Data = null,
                    //Status = false,
                    //StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    //Message = ex.Message
                };
            }
        }

        //public async Task UpdateBidder(UpdateAuctionBidderDto input)
        //{
        //    var auctionBidder = await _auctionBidderRepository.FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
        //    if (auctionBidder == null)
        //        throw new Exception("Auction bidder not found for given id");

        //    var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionId);
        //    if (auction == null)
        //        throw new Exception("Auction not found for given id");

        //    auctionBidder.AuctionId = auction.Id;
        //    auctionBidder.BidderName = input.BidderName;

        //    await _auctionBidderRepository.UpdateAsync(auctionBidder);
        //}

        public async Task DeleteBidder(Guid Id)
        {
            await _auctionBidderRepository.DeleteAsync(x => x.UniqueId == Id);
        }

        public async Task<ListResultDto<AuctionBidderListDto>> GetAllBidders()
        {
            var query = await _auctionBidderRepository.GetAllIncluding(x => x.Auction, x => x.Auction)
                                                       .Select(x => new AuctionBidderListDto
                                                       {
                                                           UniqueID = x.UniqueId,
                                                           AuctionUniqueId = x.Auction.UniqueId,
                                                           AuctionType = x.Auction.AuctionType,
                                                           AuctionEndDateTime = x.Auction.AuctionEndDateTime,
                                                           AuctionStartDateTime = x.Auction.AuctionStartDateTime,
                                                           BidderName = x.BidderName,
                                                           FullName = x.User.FullName
                                                       })
                                                      .ToListAsync();
            return new ListResultDto<AuctionBidderListDto>(query);
        }

        public async Task<ListResultDto<AuctionBidderListDto>> GetBiddersByAuctionId(Guid auctionId)
        {
            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionId);
            if (auction == null)
                throw new Exception("Auction not found for given id");

            var query = await _auctionBidderRepository.GetAllIncluding(x => x.Auction, x => x.Auction)
                                                       .Where(x => x.AuctionId == auction.Id)
                                                       .Select(x => new AuctionBidderListDto
                                                       {
                                                           UniqueID = x.UniqueId,
                                                           AuctionUniqueId = x.Auction.UniqueId,
                                                           AuctionType = x.Auction.AuctionType,
                                                           AuctionEndDateTime = x.Auction.AuctionEndDateTime,
                                                           AuctionStartDateTime = x.Auction.AuctionStartDateTime,
                                                           BidderName = x.BidderName,
                                                           FullName = x.User.FullName
                                                       })
                                                      .ToListAsync();

            return new ListResultDto<AuctionBidderListDto>(query);
        }
    }
}
