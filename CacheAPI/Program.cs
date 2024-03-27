using CacheAPI.Models;
using CacheAPI.Services;

namespace CacheAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddOptions<CacheSettings>().Bind(builder.Configuration.GetSection("CacheSettings"))
            //                                            .ValidateDataAnnotations();

            builder.Services.AddSingleton<ICacheService>(provider => 
            {
                var cacheSettings = builder.Configuration.GetSection("CacheSettings").Get<CacheSettings>();
                return new InMemoryCacheService(cacheSettings.CacheSizeLimit);
            });


            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
