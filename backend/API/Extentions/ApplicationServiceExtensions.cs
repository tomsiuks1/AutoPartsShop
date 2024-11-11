using API.Interfaces;
using API.Service;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extentions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
                });
            });
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IEmailService>(provider => new EmailService("smtp.gmail.com", 587, "tomsiuks4@gmail.com", "wtmp ufdk atrl doxl"));
            services.AddScoped<PaymentService>();

            return services;
        }
    }
}
