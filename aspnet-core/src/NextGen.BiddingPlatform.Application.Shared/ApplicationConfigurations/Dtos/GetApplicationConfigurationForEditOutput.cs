using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace NextGen.BiddingPlatform.ApplicationConfigurations.Dtos
{
    public class GetApplicationConfigurationForEditOutput
    {
        public CreateOrEditApplicationConfigurationDto ApplicationConfiguration { get; set; }

    }
}