using System.Security.Claims;

namespace APIsMainProject.Extentions
{
    public static class ClaimsPricipalExtentions
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Email);
    }
}
