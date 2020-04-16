using System.Collections.Generic;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.Editions.Dto;

namespace NextGen.BiddingPlatform.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}