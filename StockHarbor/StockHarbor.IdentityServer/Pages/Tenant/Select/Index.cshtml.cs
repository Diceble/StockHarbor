using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockHarbor.IdentityServer.Data;
using StockHarbor.IdentityServer.Interfaces;
using StockHarbor.IdentityServer.Models;
using StockHarbor.IdentityServer.Models.ViewModels;
using System.Security.Claims;

namespace StockHarbor.IdentityServer.Pages.Tenant.Select;

public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ITenantService _tenantService;


    public List<TenantViewModel> Tenants { get; set; } = new();

    [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; } = "";
    [BindProperty] public string SelectedTenantId { get; set; } = "";

    private bool IsSafeReturnUrl(string? returnUrl) =>
    !string.IsNullOrEmpty(returnUrl) &&
    (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl));


    public IndexModel(
        UserManager<ApplicationUser> userManager,
        IIdentityServerInteractionService interaction,
        ITenantService tenantService)
    {
        _userManager = userManager;
        _interaction = interaction;
        _tenantService = tenantService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!IsSafeReturnUrl(ReturnUrl)) return BadRequest();

        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Challenge(); // not logged in

        // Load the list of tenants for the user from tenantServices based on the tenantIds that the user has access to.

        //Tenants = await _db.Set<UserTenant>()
        //    .Where(ut => ut.UserId == user.Id)
        //    .Join(_db.Set<Tenant>(), ut => ut.TenantId, t => t.Id, (ut, t) => new TenantVm(t.Id.ToString(), t.Name))
        //    .OrderBy(t => t.Name)
        //    .ToListAsync();

        //if (Tenants.Count == 1)
        //{
        //    // auto-pick the only tenant
        //    SelectedTenantId = Tenants[0].Id;
        //    return await OnPostAsync();
        //}

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!_interaction.IsValidReturnUrl(ReturnUrl)) return BadRequest();
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Challenge();


        // validate selected tenant actually belongs to the user
        var ok = await _tenantService.VerifyUserTenantAsync(user.Id, SelectedTenantId);
        if (!ok) return Forbid();

        // 1) read current cookie auth (to preserve properties like persistence/expiry)
        var auth = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
            auth.Principal!.Identity!, // same identity
            auth.Principal!.Claims.Where(c => c.Type != "tenant_active"), // drop old claim
            auth.Principal!.Identity!.AuthenticationType!,
            ClaimTypes.Name, ClaimTypes.Role));

        // 2) add/replace tenant_active
        ((ClaimsIdentity)principal.Identity!).AddClaim(new Claim("tenant_active", SelectedTenantId));

        // 3) re-issue the cookie with same properties
        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal, auth.Properties);

        // 4) continue the OIDC flow
        return Redirect(ReturnUrl);
    }
}
