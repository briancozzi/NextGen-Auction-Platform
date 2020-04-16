using System;
using Abp.AutoMapper;
using NextGen.BiddingPlatform.Sessions.Dto;

namespace NextGen.BiddingPlatform.Models.Common
{
    [AutoMapFrom(typeof(ApplicationInfoDto)),
     AutoMapTo(typeof(ApplicationInfoDto))]
    public class ApplicationInfoPersistanceModel
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}