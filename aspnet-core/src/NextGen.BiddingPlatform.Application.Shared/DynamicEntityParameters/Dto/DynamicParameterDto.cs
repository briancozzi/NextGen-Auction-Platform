using Abp.Application.Services.Dto;

namespace NextGen.BiddingPlatform.DynamicEntityParameters.Dto
{
    public class DynamicParameterDto : EntityDto
    {
        public string ParameterName { get; set; }

        public string InputType { get; set; }

        public string Permission { get; set; }

        public int? TenantId { get; set; }
    }
}
