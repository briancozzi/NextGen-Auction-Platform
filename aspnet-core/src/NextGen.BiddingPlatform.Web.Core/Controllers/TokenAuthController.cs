﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetZeroCore.Web.Authentication.External;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.Notifications;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NextGen.BiddingPlatform.Authentication.TwoFactor.Google;
using NextGen.BiddingPlatform.Authorization;
using NextGen.BiddingPlatform.Authorization.Accounts.Dto;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.MultiTenancy;
using NextGen.BiddingPlatform.Web.Authentication.JwtBearer;
using NextGen.BiddingPlatform.Web.Authentication.TwoFactor;
using NextGen.BiddingPlatform.Web.Models.TokenAuth;
using NextGen.BiddingPlatform.Authorization.Impersonation;
using NextGen.BiddingPlatform.Authorization.Roles;
using NextGen.BiddingPlatform.Configuration;
using NextGen.BiddingPlatform.Debugging;
using NextGen.BiddingPlatform.Identity;
using NextGen.BiddingPlatform.Net.Sms;
using NextGen.BiddingPlatform.Notifications;
using NextGen.BiddingPlatform.Security.Recaptcha;
using NextGen.BiddingPlatform.Web.Authentication.External;
using NextGen.BiddingPlatform.Web.Common;
using NextGen.BiddingPlatform.Authorization.Delegation;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using NextGen.BiddingPlatform.Authorization.Users.Dto;
using NextGen.BiddingPlatform.Web.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using NextGen.BiddingPlatform.UserEvents;
using NextGen.BiddingPlatform.ApplicationConfigurations;
using NextGen.BiddingPlatform.Helpers;

namespace NextGen.BiddingPlatform.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : BiddingPlatformControllerBase
    {
        private const string UserIdentifierClaimType = "http://aspnetzero.com/claims/useridentifier";

        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly UserManager _userManager;
        private readonly ICacheManager _cacheManager;
        private readonly IOptions<JwtBearerOptions> _jwtOptions;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IAppNotifier _appNotifier;
        private readonly ISmsSender _smsSender;
        private readonly IEmailSender _emailSender;
        private readonly IdentityOptions _identityOptions;
        private readonly GoogleAuthenticatorProvider _googleAuthenticatorProvider;
        private readonly ExternalLoginInfoManagerFactory _externalLoginInfoManagerFactory;
        private readonly ISettingManager _settingManager;
        private readonly IJwtSecurityStampHandler _securityStampHandler;
        private readonly AbpUserClaimsPrincipalFactory<User, Role> _claimsPrincipalFactory;
        public IRecaptchaValidator RecaptchaValidator { get; set; }
        private readonly IUserDelegationManager _userDelegationManager;
        private readonly IRepository<Core.AppAccounts.AppAccount> _appAccountRepository;
        private readonly IConfigurationRoot _configurationRoot;
        private readonly IUserAppService _userAppService;
        private readonly IUserPolicy _userPolicy;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly RoleManager _roleManager;
        private readonly IRepository<UserEvent, Guid> _userEventsRepository;
        private readonly IRepository<ApplicationConfiguration> _applicationConfigRepository;
        private readonly IRepository<Core.AppAccountEvents.Event> _appAccountEventRepository;

        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            UserManager userManager,
            ICacheManager cacheManager,
            IOptions<JwtBearerOptions> jwtOptions,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
            UserRegistrationManager userRegistrationManager,
            IImpersonationManager impersonationManager,
            IUserLinkManager userLinkManager,
            IAppNotifier appNotifier,
            ISmsSender smsSender,
            IEmailSender emailSender,
            IOptions<IdentityOptions> identityOptions,
            GoogleAuthenticatorProvider googleAuthenticatorProvider,
            ExternalLoginInfoManagerFactory externalLoginInfoManagerFactory,
            ISettingManager settingManager,
            IJwtSecurityStampHandler securityStampHandler,
            AbpUserClaimsPrincipalFactory<User, Role> claimsPrincipalFactory,
            IUserDelegationManager userDelegationManager,
            IRepository<Core.AppAccounts.AppAccount> appAccountRepository,
            IWebHostEnvironment env,
            IUserAppService userAppService,
            IUserPolicy userPolicy, IPasswordHasher<User> passwordHasher,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            RoleManager roleManager,
            IRepository<UserEvent, Guid> userEventsRepository,
            IRepository<ApplicationConfiguration> applicationConfigRepository,
            IRepository<Core.AppAccountEvents.Event> appAccountEventRepository)
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _userManager = userManager;
            _cacheManager = cacheManager;
            _jwtOptions = jwtOptions;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
            _appNotifier = appNotifier;
            _smsSender = smsSender;
            _emailSender = emailSender;
            _googleAuthenticatorProvider = googleAuthenticatorProvider;
            _externalLoginInfoManagerFactory = externalLoginInfoManagerFactory;
            _settingManager = settingManager;
            _securityStampHandler = securityStampHandler;
            _identityOptions = identityOptions.Value;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            RecaptchaValidator = NullRecaptchaValidator.Instance;
            _userDelegationManager = userDelegationManager;
            _appAccountRepository = appAccountRepository;
            _configurationRoot = env.GetAppConfiguration();
            _userAppService = userAppService;
            _userPolicy = userPolicy;
            _passwordHasher = passwordHasher;
            _passwordValidators = passwordValidators;
            _roleManager = roleManager;
            _userEventsRepository = userEventsRepository;
            _applicationConfigRepository = applicationConfigRepository;
            _appAccountEventRepository = appAccountEventRepository;
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            if (UseCaptchaOnLogin())
            {
                await ValidateReCaptcha(model.CaptchaResponse);
            }

            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password,
                GetTenancyNameOrNull()
            );

            var returnUrl = model.ReturnUrl;

            if (model.SingleSignIn.HasValue && model.SingleSignIn.Value && loginResult.Result == AbpLoginResultType.Success)
            {
                loginResult.User.SetSignInToken();
                returnUrl = AddSingleSignInParametersToReturnUrl(model.ReturnUrl, loginResult.User.SignInToken, loginResult.User.Id, loginResult.User.TenantId);
            }

            //Password reset
            if (loginResult.User.ShouldChangePasswordOnNextLogin)
            {
                loginResult.User.SetNewPasswordResetCode();
                return new AuthenticateResultModel
                {
                    ShouldResetPassword = true,
                    PasswordResetCode = loginResult.User.PasswordResetCode,
                    UserId = loginResult.User.Id,
                    ReturnUrl = returnUrl
                };
            }

            //Two factor auth
            await _userManager.InitializeOptionsAsync(loginResult.Tenant?.Id);

            string twoFactorRememberClientToken = null;
            if (await IsTwoFactorAuthRequiredAsync(loginResult, model))
            {
                if (model.TwoFactorVerificationCode.IsNullOrEmpty())
                {
                    //Add a cache item which will be checked in SendTwoFactorAuthCode to prevent sending unwanted two factor code to users.
                    _cacheManager
                        .GetTwoFactorCodeCache()
                        .Set(
                            loginResult.User.ToUserIdentifier().ToString(),
                            new TwoFactorCodeCacheItem()
                        );

                    return new AuthenticateResultModel
                    {
                        RequiresTwoFactorVerification = true,
                        UserId = loginResult.User.Id,
                        TwoFactorAuthProviders = await _userManager.GetValidTwoFactorProvidersAsync(loginResult.User),
                        ReturnUrl = returnUrl
                    };
                }

                twoFactorRememberClientToken = await TwoFactorAuthenticateAsync(loginResult.User, model);
            }

            // One Concurrent Login 
            if (AllowOneConcurrentLoginPerUser())
            {
                await _userManager.UpdateSecurityStampAsync(loginResult.User);
                await _securityStampHandler.SetSecurityStampCacheItem(loginResult.User.TenantId, loginResult.User.Id, loginResult.User.SecurityStamp);
                loginResult.Identity.ReplaceClaim(new Claim(AppConsts.SecurityStampKey, loginResult.User.SecurityStamp));
            }

            var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
            var refreshToken = CreateRefreshToken(await CreateJwtClaims(loginResult.Identity, loginResult.User, tokenType: TokenType.RefreshToken));

            var appAccount = await _appAccountRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(s => s.Id == loginResult.User.AppAccountId);

            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds,
                RefreshToken = refreshToken,
                RefreshTokenExpireInSeconds = (int)_configuration.RefreshTokenExpiration.TotalSeconds,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                TwoFactorRememberClientToken = twoFactorRememberClientToken,
                UserId = loginResult.User.Id,
                ReturnUrl = returnUrl,
                AppAccountUniqueId = appAccount?.UniqueId
            };
        }

        [HttpGet]
        public async Task<string> ExternalUserLogin(Guid UniqueId, string userId, int TenantId)
        {
            var returlUrl = _configurationRoot["ExternalUserLoginSettings:ReturnUrl"];

            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Clear();

            var getUserDetailsApi = await GetConfigByKey("GetUserApi");
            getUserDetailsApi += "?uniqueId=" + UniqueId;
            getUserDetailsApi += "&userId=" + userId;
            getUserDetailsApi += "&tenantId=" + TenantId;

            var httpResponse = await _client.GetAsync(getUserDetailsApi);
            if (httpResponse.IsSuccessStatusCode)
            {
                var resultFromApi = await httpResponse.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UserDetails>(resultFromApi);

                var existingUser = await _userManager.Users.FirstOrDefaultAsync(s => s.ExternalUserId == data.ExternalUserId);

                var generalPassword = "123qwe";
                if (existingUser == null)
                {
                    var input = new CreateOrUpdateUserInput
                    {
                        User = new UserEditDto
                        {
                            EmailAddress = data.EmailAddress,
                            IsActive = true,
                            IsLockoutEnabled = false,
                            IsTwoFactorEnabled = false,
                            Name = data.FirstName,
                            Password = generalPassword,
                            PhoneNumber = "",
                            ShouldChangePasswordOnNextLogin = false,
                            Surname = data.LastName,
                            UserName = data.EmailAddress,
                            ExternalUserId = data.ExternalUserId,
                            //ExternalUserUniqueId = data.ExternalUniqueId
                        },
                        AssignedRoleNames = new List<string>() { "User" }.ToArray(),
                        SendActivationEmail = false,
                        SetRandomPassword = false
                    };


                    await _userPolicy.CheckMaxUserCountAsync(data.TenantId);
                    await CreateUser(input, data.TenantId);


                    var result = await Authenticate(new AuthenticateModel
                    {
                        UserNameOrEmailAddress = input.User.UserName,
                        Password = generalPassword,
                        SingleSignIn = true,
                        ReturnUrl = returlUrl
                    });

                    return result.ReturnUrl;
                }
                else
                {
                    await _userManager.UpdateAsync(existingUser);

                    var result = await Authenticate(new AuthenticateModel
                    {
                        UserNameOrEmailAddress = existingUser.UserName,
                        Password = generalPassword,
                        SingleSignIn = true,
                        ReturnUrl = returlUrl
                    });
                    return result.ReturnUrl;
                }
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<string> GetConfigByKey(string configKey)
        {
            var configFromDb = await _applicationConfigRepository.FirstOrDefaultAsync(s => s.ConfigKey == configKey);
            if (configFromDb == null)
                throw new UserFriendlyException(configKey + " not found in database. Please configure first!!");

            return configFromDb.ConfigValue;
        }

        [HttpPost]
        public async Task<ApiResponse<AuthenticateResultModel>> ExternalUserLoginWithEvent([FromBody] ExternalLoginWithEvent model)
        {
            var returlUrl = _configurationRoot["ExternalUserLoginSettings:ReturnUrl"];

            try
            {
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Clear();

                var getUserDetailsApi = await GetConfigByKey("GetUserApi");
                var apiUrlForError = getUserDetailsApi;

                getUserDetailsApi += "?uniqueId=" + model.UserUniqueId;
                getUserDetailsApi += "&userId=" + model.UserId;
                getUserDetailsApi += "&tenantId=" + model.TenantId;
                getUserDetailsApi += "&eventId=" + model.EventId;

                var httpResponse = await _client.GetAsync(getUserDetailsApi);
                if (httpResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        var resultFromApi = await httpResponse.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<UserDetails>(resultFromApi);

                        var existingUser = await _userManager.Users.FirstOrDefaultAsync(s => s.ExternalUserId == data.ExternalUserId);

                        var generalPassword = "123qwe";

                        var @event = await _appAccountEventRepository.FirstOrDefaultAsync(s => s.UniqueId == model.EventId);
                        if (@event == null)
                            throw new UserFriendlyException("Event not found!!!");

                        if (existingUser == null)
                        {
                            var input = new CreateOrUpdateUserInput
                            {
                                User = new UserEditDto
                                {
                                    EmailAddress = data.EmailAddress,
                                    IsActive = true,
                                    IsLockoutEnabled = false,
                                    IsTwoFactorEnabled = false,
                                    Name = data.FirstName,
                                    Password = generalPassword,
                                    PhoneNumber = data.PhoneNumber,
                                    ShouldChangePasswordOnNextLogin = false,
                                    Surname = data.LastName,
                                    UserName = data.EmailAddress,
                                    ExternalUserId = data.ExternalUserId,
                                    //ExternalUserUniqueId = data.ExternalUniqueId
                                },
                                AssignedRoleNames = new List<string>() { "User" }.ToArray(),
                                SendActivationEmail = false,
                                SetRandomPassword = false
                            };


                            await _userPolicy.CheckMaxUserCountAsync(data.TenantId);
                            await CreateUser(input, data.TenantId);

                            //add entry into user events



                            await _userEventsRepository.InsertAsync(new UserEvent
                            {
                                UserId = input.User.Id.Value,
                                EventId = @event.Id,
                                TenantId = model.TenantId
                            });


                            var result = await Authenticate(new AuthenticateModel
                            {
                                UserNameOrEmailAddress = input.User.UserName,
                                Password = generalPassword,
                                SingleSignIn = true,
                                ReturnUrl = returlUrl
                            });

                            return new ApiResponse<AuthenticateResultModel>
                            {
                                Data = result,
                                //Message = "Successfully registered.",
                                //Status = true,
                                //StatusCode = System.Net.HttpStatusCode.OK
                            };
                        }
                        else
                        {

                            existingUser.EmailAddress = data.EmailAddress;
                            existingUser.Name = data.FirstName;
                            existingUser.PhoneNumber = data.PhoneNumber;
                            existingUser.Surname = data.LastName;

                            await _userManager.UpdateAsync(existingUser);

                            //add entry into user events

                            var userEvent = await _userEventsRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(s => s.EventId == @event.Id && s.UserId == existingUser.Id);
                            if (userEvent == null)
                            {
                                await _userEventsRepository.InsertAsync(new UserEvent
                                {
                                    UserId = existingUser.Id,
                                    EventId = @event.Id,
                                    TenantId = model.TenantId
                                });
                            }

                            var result = await Authenticate(new AuthenticateModel
                            {
                                UserNameOrEmailAddress = existingUser.UserName,
                                Password = generalPassword,
                                SingleSignIn = true,
                                ReturnUrl = returlUrl
                            });
                            return new ApiResponse<AuthenticateResultModel>
                            {
                                Data = result,
                                //Message = "Successfully registered.",
                                //Status = true,
                                //StatusCode = System.Net.HttpStatusCode.OK
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new ApiResponse<AuthenticateResultModel>
                        {
                            Data = null,
                            //Message = $"Error code : 500, Message : {ex.Message}",
                            //Status = false,
                            //StatusCode = System.Net.HttpStatusCode.InternalServerError
                        };

                    }

                }
                else
                {
                    return new ApiResponse<AuthenticateResultModel>
                    {
                        Data = null,
                        //Message = $"Error code : {(int)httpResponse.StatusCode}, apiUrl= {apiUrlForError}, Message : Active session link expired or user not found!!",
                        //Status = false,
                        //StatusCode = System.Net.HttpStatusCode.InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthenticateResultModel>
                {
                    Data = null,
                    //Message = ex.Message,
                    //Status = false,
                    //StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }

        }

        private async Task CreateUser(CreateOrUpdateUserInput input, int tenantId)
        {
            var user = ObjectMapper.Map<User>(input.User); //Passwords is not mapped (see mapping configuration)
            user.TenantId = tenantId;

            //Set password
            if (input.SetRandomPassword)
            {
                var randomPassword = await _userManager.CreateRandomPassword();
                user.Password = _passwordHasher.HashPassword(user, randomPassword);
                input.User.Password = randomPassword;
            }
            else if (!input.User.Password.IsNullOrEmpty())
            {
                await _userManager.InitializeOptionsAsync(tenantId);
                foreach (var validator in _passwordValidators)
                {
                    CheckErrors(await validator.ValidateAsync(_userManager, user, input.User.Password));
                }

                user.Password = _passwordHasher.HashPassword(user, input.User.Password);
            }

            user.ShouldChangePasswordOnNextLogin = input.User.ShouldChangePasswordOnNextLogin;

            //Assign roles
            user.Roles = new Collection<UserRole>();
            foreach (var roleName in input.AssignedRoleNames)
            {
                var role = await _roleManager.GetRoleByNameAsync(roleName);
                user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
            }

            CheckErrors(await _userManager.CreateAsync(user));
            input.User.Id = user.Id;
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.
        }

        [HttpPost]
        public async Task<RefreshTokenResult> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            if (!IsRefreshTokenValid(refreshToken, out var principal))
            {
                throw new ValidationException("Refresh token is not valid!");
            }

            try
            {
                var user = _userManager.GetUser(UserIdentifier.Parse(principal.Claims.First(x => x.Type == AppConsts.UserIdentifier).Value));
                if (user == null)
                {
                    throw new UserFriendlyException("Unknown user or user identifier");
                }

                principal = await _claimsPrincipalFactory.CreateAsync(user);

                var accessToken = CreateAccessToken(await CreateJwtClaims(principal.Identity as ClaimsIdentity, user));

                return await Task.FromResult(new RefreshTokenResult(accessToken, GetEncryptedAccessToken(accessToken), (int)_configuration.AccessTokenExpiration.TotalSeconds));
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ValidationException("Refresh token is not valid!", e);
            }
        }

        private bool UseCaptchaOnLogin()
        {
            if (DebugHelper.IsDebug)
            {
                return false;
            }

            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnLogin);
        }


        [HttpGet]
        [AbpAuthorize]
        public async Task LogOut()
        {
            if (AbpSession.UserId != null)
            {
                var tokenValidityKeyInClaims = User.Claims.First(c => c.Type == AppConsts.TokenValidityKey);
                await _userManager.RemoveTokenValidityKeyAsync(_userManager.GetUser(AbpSession.ToUserIdentifier()), tokenValidityKeyInClaims.Value);
                _cacheManager.GetCache(AppConsts.TokenValidityKey).Remove(tokenValidityKeyInClaims.Value);

                if (AllowOneConcurrentLoginPerUser())
                {
                    await _securityStampHandler.RemoveSecurityStampCacheItem(AbpSession.TenantId, AbpSession.GetUserId());
                }
            }
        }

        [HttpPost]
        public async Task SendTwoFactorAuthCode([FromBody] SendTwoFactorAuthCodeModel model)
        {
            var cacheKey = new UserIdentifier(AbpSession.TenantId, model.UserId).ToString();

            var cacheItem = await _cacheManager
                .GetTwoFactorCodeCache()
                .GetOrDefaultAsync(cacheKey);

            if (cacheItem == null)
            {
                //There should be a cache item added in Authenticate method! This check is needed to prevent sending unwanted two factor code to users.
                throw new UserFriendlyException(L("SendSecurityCodeErrorMessage"));
            }

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());

            if (model.Provider != GoogleAuthenticatorProvider.Name)
            {
                cacheItem.Code = await _userManager.GenerateTwoFactorTokenAsync(user, model.Provider);
                var message = L("EmailSecurityCodeBody", cacheItem.Code);

                if (model.Provider == "Email")
                {
                    await _emailSender.SendAsync(await _userManager.GetEmailAsync(user), L("EmailSecurityCodeSubject"),
                        message);
                }
                else if (model.Provider == "Phone")
                {
                    await _smsSender.SendAsync(await _userManager.GetPhoneNumberAsync(user), message);
                }
            }

            _cacheManager.GetTwoFactorCodeCache().Set(
                    cacheKey,
                    cacheItem
                );
            _cacheManager.GetCache("ProviderCache").Set(
                "Provider",
                model.Provider
            );
        }

        [HttpPost]
        public async Task<ImpersonatedAuthenticateResultModel> ImpersonatedAuthenticate(string impersonationToken)
        {
            var result = await _impersonationManager.GetImpersonatedUserAndIdentity(impersonationToken);
            var accessToken = CreateAccessToken(await CreateJwtClaims(result.Identity, result.User));

            return new ImpersonatedAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds
            };
        }

        [HttpPost]
        public async Task<ImpersonatedAuthenticateResultModel> DelegatedImpersonatedAuthenticate(long userDelegationId, string impersonationToken)
        {
            var result = await _impersonationManager.GetImpersonatedUserAndIdentity(impersonationToken);
            var userDelegation = await _userDelegationManager.GetAsync(userDelegationId);

            if (!userDelegation.IsCreatedByUser(result.User.Id))
            {
                throw new UserFriendlyException("User delegation error...");
            }

            var expiration = userDelegation.EndTime.Subtract(Clock.Now);
            var accessToken = CreateAccessToken(await CreateJwtClaims(result.Identity, result.User, expiration), expiration);

            return new ImpersonatedAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)expiration.TotalSeconds
            };
        }

        [HttpPost]
        public async Task<SwitchedAccountAuthenticateResultModel> LinkedAccountAuthenticate(string switchAccountToken)
        {
            var result = await _userLinkManager.GetSwitchedUserAndIdentity(switchAccountToken);
            var accessToken = CreateAccessToken(await CreateJwtClaims(result.Identity, result.User));

            return new SwitchedAccountAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds
            };
        }

        [HttpGet]
        public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }

        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        {
            var externalUser = await GetExternalUserInfo(model);

            var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    {
                        var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
                        var refreshToken = CreateRefreshToken(await CreateJwtClaims(loginResult.Identity, loginResult.User, tokenType: TokenType.RefreshToken));

                        var returnUrl = model.ReturnUrl;

                        if (model.SingleSignIn.HasValue && model.SingleSignIn.Value && loginResult.Result == AbpLoginResultType.Success)
                        {
                            loginResult.User.SetSignInToken();
                            returnUrl = AddSingleSignInParametersToReturnUrl(model.ReturnUrl, loginResult.User.SignInToken, loginResult.User.Id, loginResult.User.TenantId);
                        }

                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds,
                            ReturnUrl = returnUrl,
                            RefreshToken = refreshToken,
                            RefreshTokenExpireInSeconds = (int)_configuration.RefreshTokenExpiration.TotalSeconds
                        };
                    }
                case AbpLoginResultType.UnknownExternalLogin:
                    {
                        var newUser = await RegisterExternalUserAsync(externalUser);
                        if (!newUser.IsActive)
                        {
                            return new ExternalAuthenticateResultModel
                            {
                                WaitingForActivation = true
                            };
                        }

                        //Try to login again with newly registered user!
                        loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
                        if (loginResult.Result != AbpLoginResultType.Success)
                        {
                            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                                loginResult.Result,
                                model.ProviderKey,
                                GetTenancyNameOrNull()
                            );
                        }

                        var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
                        var refreshToken = CreateRefreshToken(await CreateJwtClaims(loginResult.Identity, loginResult.User, tokenType: TokenType.RefreshToken));

                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.AccessTokenExpiration.TotalSeconds,
                            RefreshToken = refreshToken,
                            RefreshTokenExpireInSeconds = (int)_configuration.RefreshTokenExpiration.TotalSeconds
                        };
                    }
                default:
                    {
                        throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                            loginResult.Result,
                            model.ProviderKey,
                            GetTenancyNameOrNull()
                        );
                    }
            }
        }

        #region Etc

        [AbpMvcAuthorize]
        [HttpGet]
        public async Task<ActionResult> TestNotification(string message = "", string severity = "info")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            await _appNotifier.SendMessageAsync(
                AbpSession.ToUserIdentifier(),
                message,
                severity.ToPascalCase().ToEnum<NotificationSeverity>()
                );

            return Content("Sent notification: " + message);
        }

        #endregion

        private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalLoginInfo)
        {
            string username;

            using (var providerManager = _externalLoginInfoManagerFactory.GetExternalLoginInfoManager(externalLoginInfo.Provider))
            {
                username = providerManager.Object.GetUserNameFromExternalAuthUserInfo(externalLoginInfo);
            }

            var user = await _userRegistrationManager.RegisterAsync(
                externalLoginInfo.Name,
                externalLoginInfo.Surname,
                externalLoginInfo.EmailAddress,
                username,
                await _userManager.CreateRandomPassword(),
                true,
                null
            );

            user.Logins = new List<UserLogin>
            {
                new UserLogin
                {
                    LoginProvider = externalLoginInfo.Provider,
                    ProviderKey = externalLoginInfo.ProviderKey,
                    TenantId = user.TenantId
                }
            };

            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            if (userInfo.ProviderKey != model.ProviderKey)
            {
                throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
            }

            return userInfo;
        }

        private async Task<bool> IsTwoFactorAuthRequiredAsync(AbpLoginResult<Tenant, User> loginResult, AuthenticateModel authenticateModel)
        {
            if (!await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled))
            {
                return false;
            }

            if (!loginResult.User.IsTwoFactorEnabled)
            {
                return false;
            }

            if ((await _userManager.GetValidTwoFactorProvidersAsync(loginResult.User)).Count <= 0)
            {
                return false;
            }

            if (await TwoFactorClientRememberedAsync(loginResult.User.ToUserIdentifier(), authenticateModel))
            {
                return false;
            }

            return true;
        }

        private async Task<bool> TwoFactorClientRememberedAsync(UserIdentifier userIdentifier, AuthenticateModel authenticateModel)
        {
            if (!await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(authenticateModel.TwoFactorRememberClientToken))
            {
                return false;
            }

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidAudience = _configuration.Audience,
                    ValidIssuer = _configuration.Issuer,
                    IssuerSigningKey = _configuration.SecurityKey
                };

                foreach (var validator in _jwtOptions.Value.SecurityTokenValidators)
                {
                    if (validator.CanReadToken(authenticateModel.TwoFactorRememberClientToken))
                    {
                        try
                        {
                            var principal = validator.ValidateToken(authenticateModel.TwoFactorRememberClientToken, validationParameters, out _);
                            var useridentifierClaim = principal.FindFirst(c => c.Type == UserIdentifierClaimType);
                            if (useridentifierClaim == null)
                            {
                                return false;
                            }

                            return useridentifierClaim.Value == userIdentifier.ToString();
                        }
                        catch (Exception ex)
                        {
                            Logger.Debug(ex.ToString(), ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug(ex.ToString(), ex);
            }

            return false;
        }

        /* Checkes two factor code and returns a token to remember the client (browser) if needed */
        private async Task<string> TwoFactorAuthenticateAsync(User user, AuthenticateModel authenticateModel)
        {
            var twoFactorCodeCache = _cacheManager.GetTwoFactorCodeCache();
            var userIdentifier = user.ToUserIdentifier().ToString();
            var cachedCode = await twoFactorCodeCache.GetOrDefaultAsync(userIdentifier);
            var provider = _cacheManager.GetCache("ProviderCache").Get("Provider", cache => cache).ToString();

            if (provider == GoogleAuthenticatorProvider.Name)
            {
                if (!await _googleAuthenticatorProvider.ValidateAsync("TwoFactor", authenticateModel.TwoFactorVerificationCode, _userManager, user))
                {
                    throw new UserFriendlyException(L("InvalidSecurityCode"));
                }
            }
            else if (cachedCode?.Code == null || cachedCode.Code != authenticateModel.TwoFactorVerificationCode)
            {
                throw new UserFriendlyException(L("InvalidSecurityCode"));
            }

            //Delete from the cache since it was a single usage code
            await twoFactorCodeCache.RemoveAsync(userIdentifier);

            if (authenticateModel.RememberClient)
            {
                if (await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled))
                {
                    return CreateAccessToken(new[]
                        {
                            new Claim(UserIdentifierClaimType, user.ToUserIdentifier().ToString())
                        },
                        TimeSpan.FromDays(365)
                    );
                }
            }

            return null;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            return CreateToken(claims, expiration ?? _configuration.AccessTokenExpiration);
        }

        private string CreateRefreshToken(IEnumerable<Claim> claims)
        {
            return CreateToken(claims, AppConsts.RefreshTokenExpiration);
        }

        private string CreateToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                signingCredentials: _configuration.SigningCredentials,
                expires: expiration == null ?
                    (DateTime?)null :
                    now.Add(expiration.Value)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        private async Task<IEnumerable<Claim>> CreateJwtClaims(ClaimsIdentity identity, User user, TimeSpan? expiration = null, TokenType tokenType = TokenType.AccessToken)
        {
            var tokenValidityKey = Guid.NewGuid().ToString();
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == _identityOptions.ClaimsIdentity.UserIdClaimType);

            if (_identityOptions.ClaimsIdentity.UserIdClaimType != JwtRegisteredClaimNames.Sub)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value));
            }

            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(AppConsts.TokenValidityKey, tokenValidityKey),
                new Claim(AppConsts.UserIdentifier, user.ToUserIdentifier().ToUserIdentifierString()),
                new Claim(AppConsts.TokenType, tokenType.To<int>().ToString())
             });

            if (!expiration.HasValue)
            {
                expiration = tokenType == TokenType.AccessToken
                    ? _configuration.AccessTokenExpiration
                    : _configuration.RefreshTokenExpiration;
            }

            _cacheManager
                .GetCache(AppConsts.TokenValidityKey)
                .Set(tokenValidityKey, "", absoluteExpireTime: expiration);

            await _userManager.AddTokenValidityKeyAsync(
                user,
                tokenValidityKey,
                DateTime.UtcNow.Add(expiration.Value)
            );

            return claims;
        }

        private static string AddSingleSignInParametersToReturnUrl(string returnUrl, string signInToken, long userId, int? tenantId)
        {
            returnUrl += (returnUrl.Contains("?") ? "&" : "?") +
                         "accessToken=" + signInToken +
                         "&userId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(userId.ToString()));
            if (tenantId.HasValue)
            {
                returnUrl += "&tenantId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(tenantId.Value.ToString()));
            }

            return returnUrl;
        }


        private bool IsRefreshTokenValid(string refreshToken, out ClaimsPrincipal principal)
        {
            principal = null;

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidAudience = _configuration.Audience,
                    ValidIssuer = _configuration.Issuer,
                    IssuerSigningKey = _configuration.SecurityKey
                };

                foreach (var validator in _jwtOptions.Value.SecurityTokenValidators)
                {
                    if (!validator.CanReadToken(refreshToken))
                    {
                        continue;
                    }

                    try
                    {
                        principal = validator.ValidateToken(refreshToken, validationParameters, out _);

                        if (principal.Claims.FirstOrDefault(x => x.Type == AppConsts.TokenType)?.Value == TokenType.RefreshToken.To<int>().ToString())
                        {
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug(ex.ToString(), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug(ex.ToString(), ex);
            }

            return false;
        }


        private bool AllowOneConcurrentLoginPerUser()
        {
            return _settingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowOneConcurrentLoginPerUser);
        }

        private async Task ValidateReCaptcha(string captchaResponse)
        {
            var requestUserAgent = Request.Headers["User-Agent"].ToString();
            if (!requestUserAgent.IsNullOrWhiteSpace() && WebConsts.ReCaptchaIgnoreWhiteList.Contains(requestUserAgent.Trim()))
            {
                return;
            }

            await RecaptchaValidator.ValidateAsync(captchaResponse);
        }
    }
    public class UserDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int TenantId { get; set; }
        public string ExternalUserId { get; set; }
        public string ExternalUniqueId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
