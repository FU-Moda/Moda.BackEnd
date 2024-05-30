using Hangfire.Dashboard;

namespace Moda.BackEnd.API.Attributes
{
    public class DashboardNoAuthorizationFilter: IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
