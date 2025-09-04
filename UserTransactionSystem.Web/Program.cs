
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserTransactionSystem.Infrastructure.Data;
using UserTransactionSystem.Infrastructure.UnitOfWork;
using UserTransactionSystem.Services.Interfaces;
using UserTransactionSystem.Services.Mapping;
using UserTransactionSystem.Services.Services;
using UserTransactionSystem.Web.Extensions;

namespace UserTransactionSystem.Web
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Add PostgreSQL database
            builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ApplicationDbScopedFactory>();
            builder.Services.AddScoped(s => s.GetRequiredService<ApplicationDbScopedFactory>().CreateDbContext());

            // Add repositories and unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IReportingService, ReportingService>();

            // Add AutoMapper
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserTransactionSystem API", Version = "v1" });
                c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}\UserTransactionSystem.Web.xml");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserTransactionSystem API v1"));
            }

            // Add global exception handling middleware
            app.UseGlobalExceptionHandler();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Apply migrations
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    dbContext.Database.Migrate();
                }
            }

            app.Run();

        }
    }

    public partial class Program { }
}
