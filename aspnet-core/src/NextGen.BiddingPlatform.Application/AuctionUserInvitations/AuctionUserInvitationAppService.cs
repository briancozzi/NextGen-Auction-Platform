using Abp.Authorization;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.AuctionUserInvitations
{
    public class AuctionUserInvitationAppService : BiddingPlatformAppServiceBase, IAuctionUserInvitationAppService
    {
        private readonly IRepository<AuctionUserInvitation, Guid> _auctionUserInvitationRepo;
        private readonly IRepository<Core.Auctions.Auction> _auctionRepository;
        public AuctionUserInvitationAppService(IRepository<AuctionUserInvitation, Guid> auctionUserInvitationRepo,
                                               IRepository<Core.Auctions.Auction> auctionRepository)
        {
            _auctionUserInvitationRepo = auctionUserInvitationRepo;
            _auctionRepository = auctionRepository;
        }

        public async Task ShareLink(string email, Guid auctionId)
        {
            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.UniqueId == auctionId);
            if (auction == null)
                throw new Abp.UI.UserFriendlyException("Auction not found.");

            if (string.IsNullOrEmpty(email))
                throw new Abp.UI.UserFriendlyException("Please provide valid email address.");

            await _auctionUserInvitationRepo.InsertAsync(new AuctionUserInvitation
            {
                AuctionId = auctionId,
                TenantId = AbpSession.TenantId,
                Email = email
            });
        }

        [AbpAuthorize]
        public async Task<bool> IsValidLink(Guid auctionLinkId)
        {
            var auction = await _auctionRepository.FirstOrDefaultAsync(x => x.AuctionLink == auctionLinkId && x.TenantId == AbpSession.TenantId);
            if (auction == null)
                return false;

            var currUser = UserManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            if (currUser == null)
                return false;

            var auctionId = auction.UniqueId;
            var currUserEmail = currUser.EmailAddress;

            var isUserExistInAuction = (await _auctionUserInvitationRepo.GetAllListAsync()).Any(x => x.AuctionId == auctionId && x.Email == currUserEmail && x.TenantId == AbpSession.TenantId);

            return isUserExistInAuction;
        }
    }
}
