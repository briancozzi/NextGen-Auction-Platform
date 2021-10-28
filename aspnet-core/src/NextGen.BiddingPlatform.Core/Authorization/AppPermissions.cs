namespace NextGen.BiddingPlatform.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        public const string Pages_Administration_ApplicationConfigurations = "Pages.Administration.ApplicationConfigurations";
        public const string Pages_Administration_ApplicationConfigurations_Create = "Pages.Administration.ApplicationConfigurations.Create";
        public const string Pages_Administration_ApplicationConfigurations_Edit = "Pages.Administration.ApplicationConfigurations.Edit";
        public const string Pages_Administration_ApplicationConfigurations_Delete = "Pages.Administration.ApplicationConfigurations.Delete";

        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_DemoUiComponents = "Pages.DemoUiComponents";
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";
        public const string Pages_Administration_Users_Unlock = "Pages.Administration.Users.Unlock";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";
        public const string Pages_Administration_OrganizationUnits_ManageRoles = "Pages.Administration.OrganizationUnits.ManageRoles";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        public const string Pages_Administration_UiCustomization = "Pages.Administration.UiCustomization";

        public const string Pages_Administration_WebhookSubscription = "Pages.Administration.WebhookSubscription";
        public const string Pages_Administration_WebhookSubscription_Create = "Pages.Administration.WebhookSubscription.Create";
        public const string Pages_Administration_WebhookSubscription_Edit = "Pages.Administration.WebhookSubscription.Edit";
        public const string Pages_Administration_WebhookSubscription_ChangeActivity = "Pages.Administration.WebhookSubscription.ChangeActivity";
        public const string Pages_Administration_WebhookSubscription_Detail = "Pages.Administration.WebhookSubscription.Detail";
        public const string Pages_Administration_Webhook_ListSendAttempts = "Pages.Administration.Webhook.ListSendAttempts";
        public const string Pages_Administration_Webhook_ResendWebhook = "Pages.Administration.Webhook.ResendWebhook";

        public const string Pages_Administration_DynamicParameters = "Pages.Administration.DynamicParameters";
        public const string Pages_Administration_DynamicParameters_Create = "Pages.Administration.DynamicParameters.Create";
        public const string Pages_Administration_DynamicParameters_Edit = "Pages.Administration.DynamicParameters.Edit";
        public const string Pages_Administration_DynamicParameters_Delete = "Pages.Administration.DynamicParameters.Delete";

        public const string Pages_Administration_DynamicParameterValue = "Pages.Administration.DynamicParameterValue";
        public const string Pages_Administration_DynamicParameterValue_Create = "Pages.Administration.DynamicParameterValue.Create";
        public const string Pages_Administration_DynamicParameterValue_Edit = "Pages.Administration.DynamicParameterValue.Edit";
        public const string Pages_Administration_DynamicParameterValue_Delete = "Pages.Administration.DynamicParameterValue.Delete";

        public const string Pages_Administration_EntityDynamicParameters = "Pages.Administration.EntityDynamicParameters";
        public const string Pages_Administration_EntityDynamicParameters_Create = "Pages.Administration.EntityDynamicParameters.Create";
        public const string Pages_Administration_EntityDynamicParameters_Edit = "Pages.Administration.EntityDynamicParameters.Edit";
        public const string Pages_Administration_EntityDynamicParameters_Delete = "Pages.Administration.EntityDynamicParameters.Delete";

        public const string Pages_Administration_EntityDynamicParameterValue = "Pages.Administration.EntityDynamicParameterValue";
        public const string Pages_Administration_EntityDynamicParameterValue_Create = "Pages.Administration.EntityDynamicParameterValue.Create";
        public const string Pages_Administration_EntityDynamicParameterValue_Edit = "Pages.Administration.EntityDynamicParameterValue.Edit";
        public const string Pages_Administration_EntityDynamicParameterValue_Delete = "Pages.Administration.EntityDynamicParameterValue.Delete";

        //Country permission
        public const string Pages_Administration_Country = "Pages.Administration.Country";
        public const string Pages_Administration_Country_Create = "Pages.Administration.Country.Create";
        public const string Pages_Administration_Country_Edit = "Pages.Administration.Country.Edit";
        public const string Pages_Administration_Country_Delete = "Pages.Administration.Country.Delete";

        //State Permission
        public const string Pages_Administration_State = "Pages.Administration.State";
        public const string Pages_Administration_State_Create = "Pages.Administration.State.Create";
        public const string Pages_Administration_State_Edit = "Pages.Administration.State.Edit";
        public const string Pages_Administration_State_Delete = "Pages.Administration.State.Delete";

        //TENANT-SPECIFIC PERMISSIONS
        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        //Account Permission
        public const string Pages_Administration_Tenant_AppAccount = "Pages.Tenant.AppAccount";
        public const string Pages_Administration_Tenant_AppAccount_All = "Pages.Tenant.AppAccount.All";
        public const string Pages_Administration_Tenant_AppAccount_Assign = "Pages.Tenant.AppAccount.Assign";
        public const string Pages_Administration_Tenant_AppAccount_Create = "Pages.Tenant.AppAccount.Create";
        public const string Pages_Administration_Tenant_AppAccount_Edit = "Pages.Tenant.AppAccount.Edit";
        public const string Pages_Administration_Tenant_AppAccount_Delete = "Pages.Tenant.AppAccount.Delete";
        //Event Permission
        public const string Pages_Administration_Tenant_Event = "Pages.Tenant.Event";
        public const string Pages_Administration_Tenant_Event_All = "Pages.Tenant.Event.All";
        public const string Pages_Administration_Tenant_Event_Assign = "Pages.Tenant.Event.Assign";
        public const string Pages_Administration_Tenant_Event_Create = "Pages.Tenant.Event.Create";
        public const string Pages_Administration_Tenant_Event_Edit = "Pages.Tenant.Event.Edit";
        public const string Pages_Administration_Tenant_Event_Delete = "Pages.Tenant.Event.Delete";
        //Auction Permission
        public const string Pages_Administration_Tenant_Auction = "Pages.Tenant.Auction";
        public const string Pages_Administration_Tenant_Auction_Create = "Pages.Tenant.Auction.Create";
        public const string Pages_Administration_Tenant_Auction_Edit = "Pages.Tenant.Auction.Edit";
        public const string Pages_Administration_Tenant_Auction_Delete = "Pages.Tenant.Auction.Delete";

        //items permission
        public const string Pages_Administration_Tenant_Item = "Pages.Tenant.Item";
        public const string Pages_Administration_Tenant_Item_Create = "Pages.Tenant.Item.Create";
        public const string Pages_Administration_Tenant_Item_Edit = "Pages.Tenant.Item.Edit";
        public const string Pages_Administration_Tenant_Item_Delete = "Pages.Tenant.Item.Delete";

        //category permission
        public const string Pages_Administration_Tenant_Category = "Pages.Tenant.Category";
        public const string Pages_Administration_Tenant_Category_Create = "Pages.Tenant.Category.Create";
        public const string Pages_Administration_Tenant_Category_Edit = "Pages.Tenant.Category.Edit";
        public const string Pages_Administration_Tenant_Category_Delete = "Pages.Tenant.Category.Delete";

        //auction items
        public const string Pages_Administration_Tenant_AuctionItem = "Pages.Tenant.AuctionItem";
        public const string Pages_Administration_Tenant_AuctionItem_Create = "Pages.Tenant.AuctionItem.Create";
        public const string Pages_Administration_Tenant_AuctionItem_Edit = "Pages.Tenant.AuctionItem.Edit";
        public const string Pages_Administration_Tenant_AuctionItem_Delete = "Pages.Tenant.AuctionItem.Delete";

        //Auction Permission
        public const string Pages_Administration_Tenant_AuctionHistory = "Pages.Tenant.AuctionHistory";
        //public const string Pages_Administration_Tenant_AuctionHistory_Create = "Pages.Tenant.Auction.Create";
        //public const string Pages_Administration_Tenant_AuctionHistory_Edit = "Pages.Tenant.Auction.Edit";
        public const string Pages_Administration_Tenant_AuctionHistory_Delete = "Pages.Tenant.AuctionHistory.Delete";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";
        public const string Pages_Editions_MoveTenantsToAnotherEdition = "Pages.Editions.MoveTenantsToAnotherEdition";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";

    }
}