using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.BaseTypeExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAndValidateServiceOptions<T>(
        this IServiceCollection services,
        IConfigurationRoot configurationRoot
    ) where T : class
    {
        var sectionName = typeof(T).Name;
        var section = configurationRoot.GetSection(sectionName);

        var configValue = section.Get<T>();
        if (configValue is null)
        {
            throw new InvalidOperationException(
                $"Configuration section '{sectionName}' is required for {typeof(T).Name}.");
        }

        services.AddSingleton(configValue);

        services.AddOptions<T>()
            .Configure(options => section
                .Bind(options))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}