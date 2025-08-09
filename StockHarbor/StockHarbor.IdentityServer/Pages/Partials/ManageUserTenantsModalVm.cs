using StockHarbor.IdentityServer.Models.ViewModels;

namespace StockHarbor.IdentityServer.Pages.Partials;

public class ManageUserTenantsModalVm
{
    public IEnumerable<UserViewModel> Users { get; set; } = Enumerable.Empty<UserViewModel>();
}
