using System.Collections.Generic;
using MvvmHelpers;
using NextGen.BiddingPlatform.Models.NavigationMenu;

namespace NextGen.BiddingPlatform.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}