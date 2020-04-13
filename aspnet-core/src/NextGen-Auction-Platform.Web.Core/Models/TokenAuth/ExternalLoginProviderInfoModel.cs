using Abp.AutoMapper;
using NextGen-Auction-Platform.Authentication.External;

namespace NextGen-Auction-Platform.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
