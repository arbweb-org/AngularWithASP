using angularwithasp.server.Data;
using angularwithasp.server.Services;
using Microsoft.EntityFrameworkCore;

namespace angularwithasp.server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            // Add DBContext pool
            builder.Services.AddDbContextPool<StockDbContext>((options) =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Hosted service to send mails
            builder.Services.AddHostedService<StockBackgroundService>();

            var app = builder.Build();

            // Auto-migrate DB
            using (var Scope = app.Services.CreateScope())
            {
                var context = Scope.ServiceProvider.GetRequiredService<StockDbContext>();
                context.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}