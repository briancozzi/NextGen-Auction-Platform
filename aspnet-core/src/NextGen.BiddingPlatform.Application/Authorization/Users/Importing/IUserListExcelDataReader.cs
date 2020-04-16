using System.Collections.Generic;
using NextGen.BiddingPlatform.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace NextGen.BiddingPlatform.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
