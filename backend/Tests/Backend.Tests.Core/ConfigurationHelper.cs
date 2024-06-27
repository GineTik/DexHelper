using Microsoft.Extensions.Configuration;

namespace Backend.Tests.Core;

public static class ConfigurationHelper
{
    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Path.Combine(Environment.CurrentDirectory, "../../../../../Backend/"))
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
        .Build();
}