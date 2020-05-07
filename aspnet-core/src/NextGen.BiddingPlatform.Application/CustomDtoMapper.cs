using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityParameters;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using NextGen.BiddingPlatform.AppAccount.Dto;
using NextGen.BiddingPlatform.AppAccountEvent.Dto;
using NextGen.BiddingPlatform.Auction.Dto;
using NextGen.BiddingPlatform.AuctionItem.Dto;
using NextGen.BiddingPlatform.Auditing.Dto;
using NextGen.BiddingPlatform.Authorization.Accounts.Dto;
using NextGen.BiddingPlatform.Authorization.Delegation;
using NextGen.BiddingPlatform.Authorization.Permissions.Dto;
using NextGen.BiddingPlatform.Authorization.Roles;
using NextGen.BiddingPlatform.Authorization.Roles.Dto;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.Authorization.Users.Delegation.Dto;
using NextGen.BiddingPlatform.Authorization.Users.Dto;
using NextGen.BiddingPlatform.Authorization.Users.Importing.Dto;
using NextGen.BiddingPlatform.Authorization.Users.Profile.Dto;
using NextGen.BiddingPlatform.CardDetail.Dto;
using NextGen.BiddingPlatform.Category.Dto;
using NextGen.BiddingPlatform.Chat;
using NextGen.BiddingPlatform.Chat.Dto;
using NextGen.BiddingPlatform.Core.AuctionItems;
using NextGen.BiddingPlatform.Core.Categories;
using NextGen.BiddingPlatform.Core.Items;
using NextGen.BiddingPlatform.DynamicEntityParameters.Dto;
using NextGen.BiddingPlatform.Editions;
using NextGen.BiddingPlatform.Editions.Dto;
using NextGen.BiddingPlatform.Friendships;
using NextGen.BiddingPlatform.Friendships.Cache;
using NextGen.BiddingPlatform.Friendships.Dto;
using NextGen.BiddingPlatform.Items.Dto;
using NextGen.BiddingPlatform.Localization.Dto;
using NextGen.BiddingPlatform.MultiTenancy;
using NextGen.BiddingPlatform.MultiTenancy.Dto;
using NextGen.BiddingPlatform.MultiTenancy.HostDashboard.Dto;
using NextGen.BiddingPlatform.MultiTenancy.Payments;
using NextGen.BiddingPlatform.MultiTenancy.Payments.Dto;
using NextGen.BiddingPlatform.Notifications.Dto;
using NextGen.BiddingPlatform.Organizations.Dto;
using NextGen.BiddingPlatform.Sessions.Dto;
using NextGen.BiddingPlatform.State.Dto;
using NextGen.BiddingPlatform.WebHooks.Dto;

namespace NextGen.BiddingPlatform
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicParameter, DynamicParameterDto>().ReverseMap();
            configuration.CreateMap<DynamicParameterValue, DynamicParameterValueDto>().ReverseMap();
            configuration.CreateMap<EntityDynamicParameter, EntityDynamicParameterDto>()
                .ForMember(dto => dto.DynamicParameterName,
                    options => options.MapFrom(entity => entity.DynamicParameter.ParameterName));
            configuration.CreateMap<EntityDynamicParameterDto, EntityDynamicParameter>();

            configuration.CreateMap<EntityDynamicParameterValue, EntityDynamicParameterValueDto>().ReverseMap();
            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();


            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
            //Country
            configuration.CreateMap<Country.Dto.CreateCountryDto, Country.Country>().ReverseMap();
            configuration.CreateMap<Country.Dto.CountryDto, Country.Country>().ReverseMap();
            configuration.CreateMap<Country.Country, Country.Dto.CountryListDto>();

            //State
            configuration.CreateMap<CreateStateDto, Core.State.State>().ReverseMap();
            configuration.CreateMap<Core.State.State, StateDto>()
                .ForMember(x => x.CountryName, option => option.MapFrom(x => x.Country.CountryName))
                .ForMember(x => x.CountryUniqueId, option => option.MapFrom(x => x.Country.UniqueId)).ReverseMap();
            configuration.CreateMap<UpdateStateDto, Core.State.State>().ReverseMap();
            configuration.CreateMap<Core.State.State, StateListDto>()
                .ForMember(x => x.CountryName, option => option.MapFrom(c => c.Country.CountryName))
                .ForMember(x => x.CountryUniqueId, option => option.MapFrom(c => c.Country.UniqueId));

            //AppAccount
            configuration.CreateMap<CreateAppAccountDto, Core.AppAccounts.AppAccount>().ReverseMap();
            configuration.CreateMap<Core.AppAccounts.AppAccount, AppAccountDto>().ReverseMap();
            configuration.CreateMap<Core.AppAccounts.AppAccount, AppAccountListDto>()
                .ForMember(x => x.FirstName, option => option.MapFrom(a => a.FirstName))
                .ForMember(x => x.LastName, option => option.MapFrom(a => a.LastName));

            //Address
            configuration.CreateMap<Address.Dto.AddressDto, Core.Addresses.Address>().ReverseMap();
            configuration.CreateMap<Core.Addresses.Address, Address.Dto.AddressDto>()
                .ForMember(x => x.StateUniqueId, option => option.MapFrom(s => s.State.UniqueId))
                .ForMember(x => x.CountryUniqueId, option => option.MapFrom(c => c.Country.UniqueId));

            //AccountEvent
            configuration.CreateMap<CreateAccountEventDto, Core.AppAccountEvents.Event>().ReverseMap();
            configuration.CreateMap<UpdateAccountEventDto, Core.AppAccountEvents.Event>().ReverseMap();
            configuration.CreateMap<Core.AppAccountEvents.Event, AccountEventDto>()
                .ForMember(x => x.AppAccountUniqueId, option => option.MapFrom(a => a.AppAccount.UniqueId));
            configuration.CreateMap<Core.AppAccountEvents.Event, AccountEventListDto>()
                .ForMember(x => x.AppAccountUniqueId, option => option.MapFrom(a => a.AppAccount.UniqueId));

            //Auction
            configuration.CreateMap<Core.Auctions.Auction, AuctionListDto>()
                .ForMember(x => x.AuctionType, option => option.MapFrom(au => au.AuctionType))
                .ForMember(x => x.AppAccountUniqueId, option => option.MapFrom(ap => ap.AppAccount.UniqueId))
                .ForMember(x => x.EventUniqueId, option => option.MapFrom(e => e.Event.UniqueId));
            configuration.CreateMap<Core.Auctions.Auction, AuctionDto>()
                .ForMember(x => x.AppAccountUniqueId, option => option.MapFrom(ap => ap.AppAccount.UniqueId))
                .ForMember(x => x.EventUniqueId, option => option.MapFrom(e => e.Event.UniqueId));

            configuration.CreateMap<Core.Auctions.Auction, UpdateAuctionDto>()
               .ForMember(x => x.AccountUniqueId, option => option.MapFrom(ap => ap.AppAccount.UniqueId))
               .ForMember(x => x.EventUniqueId, option => option.MapFrom(e => e.Event.UniqueId));
              

            configuration.CreateMap<CreateAuctionDto, Core.Auctions.Auction>().ReverseMap();

            //Category
            configuration.CreateMap<Core.Categories.Category, CategoryListDto>();
            configuration.CreateMap<CategoryDto, Core.Categories.Category>().ReverseMap();

            //item
            configuration.CreateMap<ItemDto, Item>();
            configuration.CreateMap<UpdateItemDto, Item>();
            configuration.CreateMap<ItemGalleryDto, ItemGallery>().ReverseMap();
            configuration.CreateMap<GetItemDto, Item>().ReverseMap();

            //AuctionItem
            configuration.CreateMap<Core.AuctionItems.AuctionItem, AuctionItemDto>()
                .ForMember(x => x.AuctionId, option => option.MapFrom(a => a.Auction.UniqueId))
                .ForMember(x => x.ItemId, option => option.MapFrom(i => i.Item.UniqueId)).ReverseMap();

            //CardDetail
            configuration.CreateMap<CreateCardDetailDto, Core.CardDetails.CardDetail>();
            configuration.CreateMap<Core.CardDetails.CardDetail,CreateCardDetailDto > ()
                .ForMember(x => x.FullName, option => option.MapFrom(x => x.User.FullName));
            configuration.CreateMap<Core.CardDetails.CardDetail,CardDetailDto>()
                .ForMember(x=>x.FullName,option => option.MapFrom(x=>x.User.FullName)).ReverseMap();
        }
    }
}
