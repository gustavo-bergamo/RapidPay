using RapidPay.DependencyInjection;
using RapidPay.Domain.Authentication.Models;

namespace RapidPay.API.Shared.Models
{
    public class ApplicationUserResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        internal static IEnumerable<object> ConfigureMappings(AutomapperProfile profile)
        {
            yield return profile.CreateMap<ApplicationUser, ApplicationUserResponse>();
        }
    }
}
