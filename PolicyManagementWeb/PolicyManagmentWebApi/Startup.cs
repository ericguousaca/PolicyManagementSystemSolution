using CommonLibrary.Messaging.Constants;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PolicyManagementWebApi.Hubs;
using PolicyManagementWebApi.Messaging.Consumers;
using PolicyManagementWebApi.Models;
using PolicyManagementWebApi.Repositories;
using PolicyManagementWebApi.Services;
using System;

namespace PolicyManagementWebApi
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PolicyManagementWebApi", Version = "v1" });
            });

            services.AddScoped<ISearchPolicyService, SearchPolicyService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SearchPolicyResultCommandConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(new Uri(RabbitMqConstants.RabbitMqUri), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    config.ReceiveEndpoint(RabbitMqConstants.SearchPolicyResultCommandQueue, ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<SearchPolicyResultCommandConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();

            services.AddSignalR();

            services.Configure<MongoPolicyDbSettings>(
                Configuration.GetSection(nameof(MongoPolicyDbSettings)));

            services.AddSingleton<IMongoPolicyDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoPolicyDbSettings>>().Value);

            services.AddSingleton<ISearchPolicyRepository, MongoSearchPolicyRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PolicyManagementWebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SearchPolicyHub>("/PolicyHub");
            });
        }
    }
}
