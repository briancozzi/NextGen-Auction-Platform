using System.Collections.Generic;
using NextGen.BiddingPlatform.Auditing.Dto;
using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
