using CommonLibrary.Messaging.Constants;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PolicyApiLibrary.Builders;
using PolicyApiLibrary.DbModels;
using PolicyApiLibrary.Messaging.Consumers;
using PolicyApiLibrary.Repositories;
using PolicyApiLibrary.Services;
using System;

namespace PolicyApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PolicyApi", Version = "v1" });
            });
            services.AddDbContext<PolicyDbContext>(options =>
            {
                options.UseSqlServer(Configuration["connectionStrings:PolicyDbConnStr"]);
            });
            services.AddScoped<IUserTypeRepository, SqlUserTypeRepository>();
            services.AddScoped<IPolicyTypeRepository, SqlPolicyTypeRepository>();
            services.AddScoped<IPolicyUserTypeRepository, SqlPolicyUserTypeRepository>();
            services.AddScoped<IPolicyRepository, SqlPolicyRepository>();
            services.AddScoped<IDirector, PolicyDetailDeirector>();
            services.AddScoped<IPolicyDetailBuilder, PolicyDetailBuilder>();
            services.AddScoped<IPolicyService, PolicyService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SearchPolicyCommandConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(RabbitMqConstants.RabbitMqUri), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ReceiveEndpoint(RabbitMqConstants.SearchPolicyCommandQueue, ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<SearchPolicyCommandConsumer>(provider);

                    });
                }));
            });
            services.AddMassTransitHostedService();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .SetIsOriginAllowed((host) => true)
                       .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PolicyApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors("CorsPolicy");
            //app.UseCors(x => x
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .SetIsOriginAllowed(origin => true) // allow any origin
            //    .AllowCredentials()); // allow credentials

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
