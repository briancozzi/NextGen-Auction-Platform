using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace NextGen.BiddingPlatform.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));
            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var applicationConfigurations = administration.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationConfigurations, L("ApplicationConfigurations"), multiTenancySides: MultiTenancySides.Tenant);
            applicationConfigurations.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationConfigurations_Create, L("CreateNewApplicationConfiguration"), multiTenancySides: MultiTenancySides.Tenant);
            applicationConfigurations.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationConfigurations_Edit, L("EditApplicationConfiguration"), multiTenancySides: MultiTenancySides.Tenant);
            applicationConfigurations.CreateChildPermission(AppPermissions.Pages_Administration_ApplicationConfigurations_Delete, L("DeleteApplicationConfiguration"), multiTenancySides: MultiTenancySides.Tenant);

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicParameters = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters, L("DynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Create, L("CreatingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Edit, L("EditingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Delete, L("DeletingDynamicParameters"));

            var dynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue, L("DynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Create, L("CreatingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Edit, L("EditingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Delete, L("DeletingDynamicParameterValue"));

            var entityDynamicParameters = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters, L("EntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Create, L("CreatingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Edit, L("EditingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Delete, L("DeletingEntityDynamicParameters"));

            var entityDynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue, L("EntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create, L("CreatingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit, L("EditingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Delete, L("DeletingEntityDynamicParameterValue"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //Change Added by ALPESH
            //account permissions
            var appAccountPermisson = administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AppAccount, L("AppAccounts"), multiTenancySides: MultiTenancySides.Tenant);
            appAccountPermisson.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AppAccount_All, L("AllAppAccount"), multiTenancySides: MultiTenancySides.Tenant);
            appAccountPermisson.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AppAccount_Assign, L("AssignAppAccount"), multiTenancySides: MultiTenancySides.Tenant);
            appAccountPermisson.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AppAccount_Create, L("CreateAppAccount"), multiTenancySides: MultiTenancySides.Tenant);
            appAccountPermisson.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AppAccount_Edit, L("EditAppAccount"), multiTenancySides: MultiTenancySides.Tenant);
            appAccountPermisson.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AppAccount_Delete, L("DeleteAppAccount"), multiTenancySides: MultiTenancySides.Tenant);
            //event permissions
            var accountEventPermisson = administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Event, L("Events"), multiTenancySides: MultiTenancySides.Tenant);
            accountEventPermisson.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Event_All, L("AllEvents"), multiTenancySides: MultiTenancySides.Tenant);
            accountEventPermisson.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Event_Assign, L("AssignEvents"), multiTenancySides: MultiTenancySides.Tenant);
            accountEventPermisson.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Event_Create, L("CreateEvent"), multiTenancySides: MultiTenancySides.Tenant);
            accountEventPermisson.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Event_Edit, L("EditEvent"), multiTenancySides: MultiTenancySides.Tenant);
            accountEventPermisson.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Event_Delete, L("DeleteEvent"), multiTenancySides: MultiTenancySides.Tenant);
            //auction permission
            var auctionPermission = administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Auction, L("Auctions"), multiTenancySides: MultiTenancySides.Tenant);
            auctionPermission.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Auction_Create, L("CreateAuction"), multiTenancySides: MultiTenancySides.Tenant);
            auctionPermission.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Auction_Edit, L("EditAuction"), multiTenancySides: MultiTenancySides.Tenant);
            auctionPermission.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Auction_Delete, L("DeleteAuction"), multiTenancySides: MultiTenancySides.Tenant);
            //Auction bidder permission
            var auctionHistoryPermission = administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AuctionHistory, L("AuctionHistory"), multiTenancySides: MultiTenancySides.Tenant);
            auctionHistoryPermission.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AuctionHistory_Delete, L("DeleteAuctionHistory"), multiTenancySides: MultiTenancySides.Tenant);

            //items permission
            var itemPermissions = administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Item, L("Items"), multiTenancySides: MultiTenancySides.Tenant);
            itemPermissions.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Item_Create, L("CreateItem"), multiTenancySides: MultiTenancySides.Tenant);
            itemPermissions.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Item_Edit, L("EditItem"), multiTenancySides: MultiTenancySides.Tenant);
            itemPermissions.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Item_Delete, L("DeleteItem"), multiTenancySides: MultiTenancySides.Tenant);

            //category permission
            var categoryPermissions = administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Category, L("Categories"), multiTenancySides: MultiTenancySides.Tenant);
            categoryPermissions.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Category_Create, L("CreateCategory"), multiTenancySides: MultiTenancySides.Tenant);
            categoryPermissions.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Category_Edit, L("EditCategory"), multiTenancySides: MultiTenancySides.Tenant);
            categoryPermissions.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Category_Delete, L("DeleteCategory"), multiTenancySides: MultiTenancySides.Tenant);

            //auction item permission
            var auctionItemPermissions = administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AuctionItem, L("AuctionItems"), multiTenancySides: MultiTenancySides.Tenant);
            auctionItemPermissions.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AuctionItem_Create, L("CreateAuctionItem"), multiTenancySides: MultiTenancySides.Tenant);
            auctionItemPermissions.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AuctionItem_Edit, L("EditAuctionItem"), multiTenancySides: MultiTenancySides.Tenant);
            auctionItemPermissions.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_AuctionItem_Delete, L("DeleteAuctionItem"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS
            var country = administration.CreateChildPermission(AppPermissions.Pages_Administration_Country, L("Country"), multiTenancySides: MultiTenancySides.Host);
            country.CreateChildPermission(AppPermissions.Pages_Administration_Country_Create, L("CreateNewCountry"), multiTenancySides: MultiTenancySides.Host);
            country.CreateChildPermission(AppPermissions.Pages_Administration_Country_Edit, L("EditingCountry"), multiTenancySides: MultiTenancySides.Host);
            country.CreateChildPermission(AppPermissions.Pages_Administration_Country_Delete, L("DeletingCountry"), multiTenancySides: MultiTenancySides.Host);

            var statePermission = administration.CreateChildPermission(AppPermissions.Pages_Administration_State, L("State"), multiTenancySides: MultiTenancySides.Host);
            statePermission.CreateChildPermission(AppPermissions.Pages_Administration_State_Create, L("CreateState"), multiTenancySides: MultiTenancySides.Host);
            statePermission.CreateChildPermission(AppPermissions.Pages_Administration_State_Edit, L("EditState"), multiTenancySides: MultiTenancySides.Host);
            statePermission.CreateChildPermission(AppPermissions.Pages_Administration_State_Delete, L("DeleteState"), multiTenancySides: MultiTenancySides.Host);

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BiddingPlatformConsts.LocalizationSourceName);
        }
    }
}