using Abp.Application.Services.Dto;
using System;

namespace NextGen.BiddingPlatform.ApplicationConfigurations.Dtos
{
    public class GetAllApplicationConfigurationsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

    }
}