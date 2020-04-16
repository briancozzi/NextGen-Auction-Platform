using System.Collections.Generic;
using NextGen.BiddingPlatform.Authorization.Users.Dto;
using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}