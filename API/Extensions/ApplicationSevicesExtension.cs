using API.Data;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

public static class ApplicationSevicesExtension
{
    public static IServiceCollection AddApplicationServices( this IServiceCollection services,IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefultConnection"));
        });
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
