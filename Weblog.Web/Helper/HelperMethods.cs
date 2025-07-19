using System.Globalization;
using System.Security.Claims;

namespace Weblog.Web.Helper;

public static class HelperMethods
{
    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        if (user.Identity.IsAuthenticated)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var usernameClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (userIdClaim != "1" || usernameClaim?.ToLower() != "admin")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public static string ToShamsi(this DateTime Value)
    {
        PersianCalendar pc = new PersianCalendar();
        return $"{pc.GetYear(Value)}/{pc.GetMonth(Value).ToString("00")}/{pc.GetDayOfMonth(Value).ToString("00")}";
    }
}