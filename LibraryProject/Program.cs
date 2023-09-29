using webapi;

internal class Program
{
    private static void Main(string[] args)
    {
        CreateHostBuilder(args).ConfigureAppConfiguration(builder =>
        {
            builder.AddIniFile("config.ini");
        }).Build().Run();

    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}