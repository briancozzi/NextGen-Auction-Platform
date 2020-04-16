using System.Collections.Generic;
using NextGen.BiddingPlatform.Authorization.Users.Importing.Dto;
using NextGen.BiddingPlatform.Dto;

namespace NextGen.BiddingPlatform.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
