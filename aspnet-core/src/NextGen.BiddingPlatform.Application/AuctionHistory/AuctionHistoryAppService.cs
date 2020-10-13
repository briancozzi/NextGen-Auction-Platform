using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NextGen.BiddingPlatform.AuctionHistory.Dto;
using NextGen.BiddingPlatform.Authorization;
using NextGen.BiddingPlatform.Authorization.Permissions;
using NextGen.BiddingPlatform.Authorization.Roles;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.EntityFrameworkCore;
using NextGen.BiddingPlatform.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NextGen.BiddingPlatform.Enums.ItemEnums;

namespace NextGen.BiddingPlatform.AuctionHistory
{
    public class AuctionHistoryAppService : BiddingPlatformAppServiceBase, IAuctionHistoryAppService
    {
        private readonly IRepository<Core.AuctionHistories.AuctionHistory> _auctionHistoryRepository;
        private readonly IRepository<Core.AuctionBidders.AuctionBidder> _auctionBidderRepository;
        private readonly IRepository<Core.AuctionItems.AuctionItem> _auctionItemRepository;
        private readonly UserManager _userManager;
        private readonly RabbitMqSettings _rabbitMqSettings;
        public AuctionHistoryAppService(IRepository<Core.AuctionHistories.AuctionHistory> auctionHistoryRepository,
                                        IRepository<Core.AuctionBidders.AuctionBidder> auctionBidderRepository,
                                        IRepository<Core.AuctionItems.AuctionItem> auctionItemRepository,
                                        UserManager userManager,
                                        IOptions<RabbitMqSettings> rabbitMqSettings)
        {
            _auctionHistoryRepository = auctionHistoryRepository;
            _auctionBidderRepository = auctionBidderRepository;
            _auctionItemRepository = auctionItemRepository;
            _userManager = userManager;
            _rabbitMqSettings = rabbitMqSettings.Value;
        }

        public async Task CreateHistory(CreateAuctionHistoryDto input)
        {
            var auctionBidder = await _auctionBidderRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionBidderId);
            if (auctionBidder == null)
                throw new Exception("Auction Bidder not found for given id");

            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == input.AuctionItemId);
            if (auctionItem == null)
                throw new Exception("Auction Item not found for given id");

            await _auctionHistoryRepository.InsertAsync(new Core.AuctionHistories.AuctionHistory
            {
                UniqueId = Guid.NewGuid(),
                TenantId = AbpSession.TenantId.Value,
                AuctionBidderId = auctionBidder.Id,
                AuctionItemId = auctionItem.Id,
                BidAmount = input.BidAmount,
                BidStatus = input.BidStatus
            });
        }

        public async Task<ListResultDto<AuctionHistoryListDto>> GetAllHistory()
        {
            var historyData = _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .OrderByDescending(x => x.Id)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       });

            return new ListResultDto<AuctionHistoryListDto>(await historyData.ToListAsync());
        }

        public async Task<ListResultDto<AuctionHistoryListDto>> GetHistoryByBidderId(Guid auctionBidderId)
        {
            var auctionBidder = await _auctionBidderRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionBidderId);

            var historyData = _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .OrderByDescending(x => x.Id)
                                                       .Where(x => x.AuctionBidderId == auctionBidder.Id)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       });

            return new ListResultDto<AuctionHistoryListDto>(await historyData.ToListAsync());
        }

        public async Task<ListResultDto<AuctionHistoryListDto>> GetHistoryByIds(Guid auctionBidderID, Guid auctionItemId)
        {
            var auctionBidder = await _auctionBidderRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionBidderID);
            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionItemId);
            var historyData = _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .OrderByDescending(x => x.Id)
                                                       .Where(x => x.AuctionItemId == auctionItem.Id && x.AuctionBidderId == auctionBidder.Id)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       });

            return new ListResultDto<AuctionHistoryListDto>(await historyData.ToListAsync());
        }
        public async Task<ListResultDto<GetAuctionHistoryByAuctionIdDto>> GetHistorbyAuctionItemId(Guid auctionItemId)
        {
            var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionItemId);
            if (auctionItem == null)
                throw new UserFriendlyException("Auction item not found for given id");

            var historyData = _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item).AsNoTracking()
                                                       .Where(x => x.AuctionItemId == auctionItem.Id)
                                                       .OrderByDescending(x => x.CreationTime)
                                                       .Select(x => new GetAuctionHistoryByAuctionIdDto
                                                       {
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BiddingDate = x.CreationTime.ToString("MM/dd/yyyy"),
                                                           BiddingTime = x.CreationTime.ToString("hh:mm tt"),
                                                           BidHistoryDate = x.CreationTime
                                                       });

            return new ListResultDto<GetAuctionHistoryByAuctionIdDto>(await historyData.ToListAsync());
        }

        //Get Auction Item History by AuctionItemId
        public async Task<AuctionHistoryListDto> GetHistoryByAuctionHistoryId(Guid Id)
        {

            var historyData = await _auctionHistoryRepository.GetAllIncluding(x => x.AuctionBidder, x => x.AuctionItem, x => x.AuctionItem.Item)
                                                       .Select(x => new AuctionHistoryListDto
                                                       {
                                                           UniqueId = x.UniqueId,
                                                           BidderId = x.AuctionBidder.UniqueId,
                                                           AuctionItemId = x.AuctionItem.UniqueId,
                                                           BidAmount = x.BidAmount,
                                                           BidderName = x.AuctionBidder.BidderName,
                                                           BidStatus = x.BidStatus,
                                                           ItemName = x.AuctionItem.Item.ItemName,
                                                           ItemType = x.AuctionItem.Item.ItemType
                                                       }).FirstOrDefaultAsync(x => x.UniqueId == Id);

            return historyData;
        }

        //custom Method
        [AllowAnonymous]
        public async Task<GetAuctionBidderHistoryDto> SaveAuctionBidderWithHistory(AuctionBidderHistoryDto auctionBidderHistory)
        {
            try
            {
                var auctionItem = await _auctionItemRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionBidderHistory.AuctionItemId);
                if (auctionItem == null)
                    throw new UserFriendlyException("Auction item not found for bidding");

                var currUserId = 3;
                var tenantId = 2;

                var currentUser = await _userManager.GetUserAsync(new Abp.UserIdentifier(tenantId, currUserId));
                if (currentUser == null)
                    throw new UserFriendlyException("User not found");

                var userhasBidderRole = await _userManager.IsInRoleAsync(currentUser, "Bidder");
                var historyCount = await GetAuctionItemHistoryCount(auctionItem.Id);

                if (/*!userhasBidderRole &&*/ historyCount == 0)
                {
                    //CheckErrors(await _userManager.AddToRoleAsync(currentUser, "Bidder"));

                    //now add auction bidder
                    var auctionBidder = new Core.AuctionBidders.AuctionBidder
                    {
                        AuctionId = auctionItem.AuctionId,
                        BidderName = auctionBidderHistory.BidderName,
                        CreationTime = Clock.Now,
                        CreatorUserId = currUserId,
                        TenantId = auctionItem.TenantId,
                        UserId = currUserId,
                        UniqueId = Guid.NewGuid()
                    };
                    auctionBidder.AuctionHistories.Add(new Core.AuctionHistories.AuctionHistory
                    {
                        AuctionItemId = auctionItem.Id,
                        BidAmount = auctionBidderHistory.BidAmount,
                        BidStatus = BiddingStatus.Pending.ToString(),
                        CreationTime = Clock.Now,
                        CreatorUserId = currUserId,
                        TenantId = auctionItem.TenantId,
                        UniqueId = Guid.NewGuid()
                    });
                    await _auctionBidderRepository.InsertAsync(auctionBidder);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    auctionBidderHistory.AuctionBidderId = auctionBidder.Id;
                }
                else
                {
                    if (!auctionBidderHistory.AuctionBidderId.HasValue)
                        throw new UserFriendlyException("Bidder not found");

                    await _auctionHistoryRepository.InsertAsync(new Core.AuctionHistories.AuctionHistory
                    {
                        AuctionItemId = auctionItem.Id,
                        BidAmount = auctionBidderHistory.BidAmount,
                        BidStatus = BiddingStatus.Pending.ToString(),
                        CreationTime = Clock.Now,
                        CreatorUserId = currUserId,
                        TenantId = auctionItem.TenantId,
                        UniqueId = Guid.NewGuid(),
                        AuctionBidderId = auctionBidderHistory.AuctionBidderId.Value
                    });
                    await CurrentUnitOfWork.SaveChangesAsync();
                }


                return new GetAuctionBidderHistoryDto
                {
                    AuctionBidderId = auctionBidderHistory.AuctionBidderId.Value,
                    BidderName = auctionBidderHistory.BidderName,
                    HistoryCount = await GetAuctionItemHistoryCount(auctionItem.Id)
                };
            }
            catch (Exception ex)
            {
                throw;
            }
            //for now Tenant = Alpesh, UserId=3, TenantId = 2

        }
        private async Task<int> GetAuctionItemHistoryCount(int auctionItemId)
        {
            var auctionItemHistory = await _auctionHistoryRepository.GetAllListAsync(x => x.AuctionItemId == auctionItemId);
            return auctionItemHistory.Count;
        }

        public void TestSend()
        {
            ConnectionFactory factory;
            if (_rabbitMqSettings.Hostname == "localhost")
                factory = new ConnectionFactory() { HostName = _rabbitMqSettings.Hostname };
            else
                factory = new ConnectionFactory() { HostName = _rabbitMqSettings.Hostname, UserName = _rabbitMqSettings.UserName, Password = _rabbitMqSettings.Password };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _rabbitMqSettings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                //var json = JsonConvert.SerializeObject(customer);
                //var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: _rabbitMqSettings.QueueName, basicProperties: null, body: Encoding.UTF8.GetBytes("Hello World!"));
            }
        }
        public string TestGet()
        {
            ConnectionFactory factory;
            if (_rabbitMqSettings.Hostname == "localhost")
                factory = new ConnectionFactory() { HostName = _rabbitMqSettings.Hostname };
            else
                factory = new ConnectionFactory() { HostName = _rabbitMqSettings.Hostname, UserName = _rabbitMqSettings.UserName, Password = _rabbitMqSettings.Password };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            BasicGetResult result = channel.BasicGet(_rabbitMqSettings.QueueName, true);
            if (result != null) return Encoding.UTF8.GetString(result.Body.ToArray());
            else return "";
        }
    }
}
