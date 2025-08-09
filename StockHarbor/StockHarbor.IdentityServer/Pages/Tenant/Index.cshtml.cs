using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockHarbor.IdentityServer.Interfaces;
using StockHarbor.IdentityServer.Models.DTO;
using StockHarbor.IdentityServer.Models.ViewModels;

namespace StockHarbor.IdentityServer.Pages.Tenant;

public class IndexModel(ITenantService tenantService, IUserService userService) : PageModel
{
    public IReadOnlyList<TenantDto> Tenants { get; set; } = [];
    public IReadOnlyList<UserViewModel> Users { get; set; } = [];
    public IReadOnlyList<UserTenantViewModel> UserTenants { get; set; } = [];

    [BindProperty]
    public string TenantId { get; set; } = string.Empty;

    [BindProperty]
    public List<string> SelectedUserIds { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        Tenants = await tenantService.GetAllTenantsAsync(ct);
        Users = await userService.GetAllUsersAsync(ct);
        UserTenants = await userService.GetAllUserTenantsAsync(ct);
        return Page();
    }

    public async Task<IActionResult> OnPostSaveTenantUsersAsync(CancellationToken ct)
    {
        try
        {
            await tenantService.SyncUsersForTenant(Guid.Parse(TenantId), SelectedUserIds, ct);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred while saving tenant users: {ex.Message}");
            return Page();
        }
        return RedirectToPage();
    }
}
