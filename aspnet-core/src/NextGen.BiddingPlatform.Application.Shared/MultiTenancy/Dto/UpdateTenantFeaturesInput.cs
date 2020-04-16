using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace NextGen.BiddingPlatform.MultiTenancy.Dto
{
    public class UpdateTenantFeaturesInput
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public List<NameValueDto> FeatureValues { get; set; }
    }
}