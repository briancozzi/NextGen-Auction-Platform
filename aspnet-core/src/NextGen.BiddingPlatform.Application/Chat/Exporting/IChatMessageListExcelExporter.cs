using System.Collections.Generic;
using Abp;
using NextGen.BiddingPlatform.Chat.Dto;
using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
