namespace NextGen.BiddingPlatform.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}