using AutoMapper;
using System.Reflection;

namespace RapidPay.DependencyInjection
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            var assembly = Assembly.GetEntryAssembly();
            MapProfiles(assembly!);
        }

        private void MapProfiles(Assembly assembly)
        {
            var methods = assembly!.GetTypes()
                              .SelectMany(m => m.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
                              .Where(m => m.Name == "ConfigureMappings");

            foreach (var method in methods)
            {
                var expressions = method.Invoke(null, new[] { this }) as IEnumerable<object>;
                if (expressions is not null)
                {
                    foreach (var expression in expressions)
                    {
                        expression.GetType().GetGenericArguments();
                    }
                }
            }
        }
    }
}
