using StockHarbor.TenantApi.Models.enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockHarbor.TenantApi.Models.Entities;

public class Tenant
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid TenantId { get; set; }
    public required string TenantName { get; set; }
    public required string ConnectionString { get; set; }
    public required DateTimeOffset CreatedDate { get; set; }
    public required TenantStatus Status { get; set; } 
    public string? ContactEmail { get; set; }
}
