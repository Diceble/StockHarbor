namespace StockHarbor.TenantApi.Models.enums;

public enum TenantStatus
{
    /// <summary>
    /// Active means the tenant is in active operation
    /// </summary>
    Active = 0,
    /// <summary>
    /// Suspended means the tenant is not able to accessed for a certain amound of time
    /// </summary>
    Suspended = 1,
    /// <summary>
    /// Disabled means the tenant is not able to be accessed for infinite amount of time
    /// </summary>
    Disabled = 2,
    /// <summary>
    /// Deleted mean the tenant is soft deleted and will be deleted after a certain amount of days
    /// </summary>
    Deleted = 3,
    /// <summary>
    /// Pending means there are still background progressing going on to fully create the tenant
    /// </summary>
    Pending = 4
}
