using KLG.Backend.Organization.Services.Configuration;
using KLG.Backend.Organization.Services.Entities;
using KLG.Library.Microservice;

namespace KLG.Backend.Organization.Services;

public static class Organization
{
    public async static Task Main(string[] args)
    {
        // initialize auto-mapper
        KLGMapper.Initialize();

        // cerate new microservice builder
        var builder = new KLGMicroserviceBuilder<DefaultDbContext>();

        // add your own services
        //builder.Services.AddScoped<...> ();

        // add your own configuration classes
        // you can add more than one custom configurations
        builder.RegisterConfigurationSection<OrganizationConfiguration>();

        // run the microservice
        await builder.Build().RunAsync();

    }

}
