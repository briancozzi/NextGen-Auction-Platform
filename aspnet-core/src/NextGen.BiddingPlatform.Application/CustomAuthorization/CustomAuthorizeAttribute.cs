using Abp.Application.Services.Dto;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NextGen.BiddingPlatform.AppAccount;
using NextGen.BiddingPlatform.AppAccount.Dto;
using NextGen.BiddingPlatform.Authorization.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static NextGen.BiddingPlatform.CustomAuthorization.CustomEnum;

namespace NextGen.BiddingPlatform.CustomAuthorization
{
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute(string permission, Permission servicePermission, ServiceType serviceType) : base(typeof(CustomAuthorizeFilter))
        {
            Arguments = new object[] { permission, servicePermission, serviceType };
        }
    }

    public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly ServiceType _serviceType;
        private readonly string _permission;
        private readonly Permission _servicePermission;
        private List<string> grantedPermissions = new List<string>();
        private List<string> servicePermissions = new List<string>();
        private readonly IAbpSession _abpSession;
        private readonly IUserAppService _userAppService;
        private readonly IAppAccountAppService _appAccountService;
        public CustomAuthorizeFilter(string permission,
                                     Permission servicePermission,
                                     ServiceType serviceType,
                                     IAbpSession abpSession,
                                     IUserAppService userAppSevicer,
                                     IAppAccountAppService appAccountService)
        {
            _serviceType = serviceType;
            _permission = permission;
            _servicePermission = servicePermission;
            _abpSession = abpSession;
            _userAppService = userAppSevicer;
            _appAccountService = appAccountService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Guid Id = Guid.Empty;
            if (context.HttpContext.Request.Method?.ToUpper() == "POST" && _servicePermission == Permission.Edit)
            {
                Id = await GetUniqueId(context, _serviceType);
            }
            else
            {
                var id = context.HttpContext.Request.Query["Id"].ToString();
                Id = Guid.Parse(id);
            }

            var currentUserId = _abpSession.UserId;
            if (currentUserId == null)
            {
                context.Result = new ObjectResult((int)HttpStatusCode.Forbidden);
                return;
            }

            var userID = new EntityDto<long>(currentUserId.Value);
            var permmissions = await _userAppService.GetUserPermissionsForEdit(userID);
            grantedPermissions = permmissions.GrantedPermissionNames;

            if (_serviceType == ServiceType.AppAccount)
            {
                //servicePermissions = await _appAccountService.GetAppAccountPermission(currentUserId.Value, Id)
            }

            var hasPermissionGranted = grantedPermissions.Contains(_permission);
            var hasServicePermission = servicePermissions.Contains(_servicePermission.ToString());

            if (hasPermissionGranted || hasServicePermission)
                return;
            else
            {
                context.Result = new ObjectResult((int)HttpStatusCode.Forbidden);
                return;
            }
        }

        public async Task<Guid> GetUniqueId(AuthorizationFilterContext context, ServiceType type)
        {
            object data;
            using (var body = new StreamReader(context.HttpContext.Request.Body))
            {
                var bodyData = await body.ReadToEndAsync();
                data = JsonConvert.DeserializeObject<object>(bodyData);
            }
            if (type == ServiceType.AppAccount)
            {
                var account = data as UpdateAppAccountDto;
                return account.UniqueId;
            }
            return Guid.Empty;
        }
    }
}
