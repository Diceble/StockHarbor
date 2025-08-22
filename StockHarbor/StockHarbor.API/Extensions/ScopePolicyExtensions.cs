using Microsoft.AspNetCore.Authorization;

namespace StockHarbor.API.Extensions;

public static class ScopePolicyExtensions
{
    public static AuthorizationPolicyBuilder RequireAnyScope(
       this AuthorizationPolicyBuilder policy,
       params string[] requiredScopes)
    {
        return policy.RequireAssertion(ctx =>
        {
            if (ctx.User?.Identity?.IsAuthenticated != true)
                return false;

            // Collect scope values from both "scope" and "scp"
            var scopeValues = ctx.User.FindAll("scope").Select(c => c.Value)
                               .Concat(ctx.User.FindAll("scp").Select(c => c.Value));

            // Split space-separated scope strings and flatten
            var granted = scopeValues
                .SelectMany(v => v.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .ToHashSet(StringComparer.Ordinal);

            return requiredScopes.Any(granted.Contains);
        });
    }
}
