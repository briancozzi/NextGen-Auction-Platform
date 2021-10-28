using System;
using Abp.Application.Services.Dto;

namespace NextGen.BiddingPlatform.ApplicationConfigurations.Dtos
{
    public class ApplicationConfigurationDto : EntityDto
    {
        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

    }
}