
using BorsaApi.DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BorsaApi
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost3000",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            // Diðer yapýlandýrmalar...

        // Controllers eklemek için AddControllers'ý kullanýn
        services.AddControllers();

            // HttpClient eklemek için AddHttpClient'ý kullanýn
            services.AddHttpClient();

            // Swagger eklemek için AddSwaggerGen'ý kullanýn
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BorsaApi", Version = "v1" });
            });

            // MongoDB baðlantýsýný yapýlandýrýn ve IMongoClient hizmetini ekleyin
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            services.AddSingleton<IMongoClient>(mongoClient);

            // BorsaApiDatabaseSettings yapýlandýrmasýný ekleyin
            services.Configure<BorsaApiDatabaseSettings>(
                Configuration.GetSection(nameof(BorsaApiDatabaseSettings)));

            // IBorsaApiDatabaseSettings hizmetini ekleyin
            services.AddSingleton<IBorsaApiDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<BorsaApiDatabaseSettings>>().Value);

            // Kullanýcý hizmetini ekleyin (UserService örneði)
            services.AddScoped<IUserService, UserService>();
   
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowLocalhost3000");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BorsaApi v1"));
            }

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