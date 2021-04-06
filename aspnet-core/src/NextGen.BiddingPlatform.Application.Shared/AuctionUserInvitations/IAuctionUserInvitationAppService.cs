using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.AuctionUserInvitations
{
    public interface IAuctionUserInvitationAppService : IApplicationService
    {
        Task ShareLink(string email, Guid auctionId);
        Task<bool> IsValidLink(Guid auctionLinkId);
    }
}
