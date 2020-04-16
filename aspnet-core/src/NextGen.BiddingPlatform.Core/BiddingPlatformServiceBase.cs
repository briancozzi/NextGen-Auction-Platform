using Abp;

namespace NextGen.BiddingPlatform
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="BiddingPlatformDomainServiceBase"/>.
    /// For application services inherit BiddingPlatformAppServiceBase.
    /// </summary>
    public abstract class BiddingPlatformServiceBase : AbpServiceBase
    {
        protected BiddingPlatformServiceBase()
        {
            LocalizationSourceName = BiddingPlatformConsts.LocalizationSourceName;
        }
    }
}