using Abp.Application.Services;
using NextGen.BiddingPlatform.Dto;
using NextGen.BiddingPlatform.Logging.Dto;

namespace NextGen.BiddingPlatform.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
