using BCVP.Net8.Common.Option;
using Microsoft.Extensions.DependencyInjection;

namespace BCVP.Net8.Extension
{
    public static class AllOptionRegister
    {
        public static void AddAllOptionRegister(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            foreach (var optionType in typeof(ConfigurableOptions).Assembly.GetTypes().Where(s =>
                         !s.IsInterface//不是接口 
                         && typeof(IConfigurableOptions).IsAssignableFrom(s)))//并且以IConfigurableOptions接口为底层类的才会
            {
                services.AddConfigurableOptions(optionType);
            }
        }
    }
}
