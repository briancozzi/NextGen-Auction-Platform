using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
