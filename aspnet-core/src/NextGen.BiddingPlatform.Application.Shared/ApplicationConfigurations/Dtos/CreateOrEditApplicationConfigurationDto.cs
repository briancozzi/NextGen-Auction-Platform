using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace NextGen.BiddingPlatform.ApplicationConfigurations.Dtos
{
    public class CreateOrEditApplicationConfigurationDto : EntityDto<int?>
    {

        [Required]
        public string ConfigKey { get; set; }

        [Required]
        public string ConfigValue { get; set; }

    }
}