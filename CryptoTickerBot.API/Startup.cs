using CryptoTickerBot.Data;
using CryptoTickerBot.Data.Configs;
using CryptoTickerBot.Data.Contracts;
using CryptoTickerBot.Data.Converters;
using CryptoTickerBot.Data.Domain;
using CryptoTickerBot.Data.Extensions;
using CryptoTickerBot.Data.Helpers;
using CryptoTickerBot.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.ComponentModel;

namespace CryptoTickerBot.API
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
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => {
                //c.UseAllOfForInheritance()
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Erky Crypto Bot API",
                    Version = "v1",
                    Description = "Provide a flow between crypto Bots and Erky Database",
                    Contact = new OpenApiContact
                    {
                        Name = "Gastón Franzé",
                        Email = "lafondadebaco@gmail.com",
                    }
                }); 
                c.UseAllOfToExtendReferenceSchemas();

            });

            services.AddDbContext<CryptoTickerContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CryptoTicker"), providerOptions => providerOptions.EnableRetryOnFailure()));

            services.AddControllers(
                options =>
                    options.Filters.Add(new HttpResponseExceptionFilter())
                    ).AddJsonOptions(options => {
                        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());   // ver este error
                        options.JsonSerializerOptions.MaxDepth = 0;

                    });

            //Erky Crypto Bot
            services.AddScoped<ICryptoTickerHistoryRepository, CryptoTickerHistoryRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                //app.UseExceptionHandler("/error");
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();


            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Crypto Ticker API v1");
                    c.RoutePrefix = string.Empty;
                    c.DefaultModelsExpandDepth(1);
                    c.DefaultModelRendering(ModelRendering.Model);

                });

            var loggingOptions = this.Configuration.GetSection("Log4NetCore")
                                  .Get<Log4NetProviderOptions>();

            loggerFactory.AddLog4Net(loggingOptions);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}