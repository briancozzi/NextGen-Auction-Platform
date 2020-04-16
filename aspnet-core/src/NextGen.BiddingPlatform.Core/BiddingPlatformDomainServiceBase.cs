using Abp.Domain.Services;

namespace NextGen.BiddingPlatform
{
    public abstract class BiddingPlatformDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected BiddingPlatformDomainServiceBase()
        {
            LocalizationSourceName = BiddingPlatformConsts.LocalizationSourceName;
        }
    }
}
