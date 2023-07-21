namespace GimenaCreations.Constants;

public static class Permissions
{    
    public static class Admin
    {
        public const string View = "Permissions.Admin.View";
    }

    public static class AuditEntries
    {
        public const string View = "Permissions.AuditEntries.View";
    }

    public static class CatalogItems
    {
        public const string View = "Permissions.CatalogItems.View";
        public const string Create = "Permissions.CatalogItems.Create";
        public const string Edit = "Permissions.CatalogItems.Edit";
        public const string Delete = "Permissions.CatalogItems.Delete";
    }

    public static class CatalogTypes
    {
        public const string View = "Permissions.CatalogTypes.View";
        public const string Create = "Permissions.CatalogTypes.Create";
        public const string Edit = "Permissions.CatalogTypes.Edit";
        public const string Delete = "Permissions.CatalogTypes.Delete";
    }

    public static class LoginLogoutReport
    {
        public const string View = "Permissions.LoginLogoutReport.View";
    }

    public static class OrderItems
    {
        public const string Edit = "Permissions.OrderItems.Edit";
    }

    public static class Orders
    {
        public const string View = "Permissions.Orders.View";
        public const string Edit = "Permissions.Orders.Edit";
    }    

    public static class PurchaseReceptions
    {
        public const string AddItems = "Permissions.PurchaseReceptions.AddItems";
        public const string Create = "Permissions.PurchaseReceptions.Create";
        public const string Delete = "Permissions.PurchaseReceptions.Delete";
        public const string View = "Permissions.PurchaseReceptions.View";
        public const string Edit = "Permissions.PurchaseReceptions.Edit";
    }

    public static class Purchases
    {
        public const string AddProducts = "Permissions.Purchases.AddProducts";
        public const string Create = "Permissions.Purchases.Create";
        public const string Delete = "Permissions.Purchases.Delete";
        public const string View = "Permissions.Purchases.View";
        public const string Edit = "Permissions.Purchases.Edit";
    }

    public static class PurchasesChangeHistory
    {
        public const string View = "Permissions.PurchasesChangeHistory.View";
    }

    public static class Roles
    {
        public const string Create = "Permissions.Roles.Create";
        public const string Delete = "Permissions.Roles.Delete";
        public const string Edit = "Permissions.Roles.Edit";
        public const string View = "Permissions.Roles.View";
        public const string Permissions = "Permissions.Permissions.View";
    }

    public static class Suppliers
    {
        public const string Create = "Permissions.Suppliers.Create";
        public const string Delete = "Permissions.Suppliers.Delete";
        public const string Edit = "Permissions.Suppliers.Edit";
        public const string View = "Permissions.Suppliers.View";
    }

    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string ChangeStatus = "Permissions.Users.ChangeStatus";
    }

    public static class CriticalStock
    {
        public const string View = "Permissions.CriticalStock.View";
    }

    public static class HealthCheck
    {
        public const string View = "Permissions.HealthCheck.View";
    }
}
