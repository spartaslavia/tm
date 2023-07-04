using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Core.Services.Interfaces;
using TranslationManagement.Api.Core.Services;
using External.ThirdParty.Services;
using TranslationManagement.Api.Data.DbContexts;
using TranslationManagement.Api.Data.Repositories.Interfaces;
using TranslationManagement.Api.Data.Repositories;
using FluentValidation.AspNetCore;
using TranslationManagement.Api.Core.Mapping;

namespace TranslationManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddAutoMapper(c => c.AddProfile<TmProfile>(), typeof(Startup));

            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<ITranslatorRepository, TranslatorRepository>();
            services.AddTransient<ITranslationJobService, TranslationJobService>();
            services.AddTransient<ITranslatorService, TranslatorService>();

            services.AddTransient<ITranslationJobService, TranslationJobService>();
            services.AddTransient<INotificationService, UnreliableNotificationService>();
            services.AddTransient<ITranslatorService, TranslatorService>();

            services.AddCors(o => o.AddPolicy("EnableCORS", builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TranslationManagement.Api", Version = "v1" });
            });
            
            services.AddControllers();
            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlite("Data Source=TranslationAppDatabase.db"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TranslationManagement.Api v1"));

            app.UseRouting();
            app.UseCors("EnableCORS");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
