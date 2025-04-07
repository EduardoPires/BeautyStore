using BeautyStore.Infra.Data.Contexts;
using BeautyStore.MVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace BeautyStore.MVC.Configurations
{
    public static class DatabaseSelectorExtension
    {
        public static void AddDatabaseSelector(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<BeautyStoreDbContext>(options =>
                  options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite"))
                    .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite"))
                      .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));
            }
            else
            {
                builder.Services.AddDbContext<BeautyStoreDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            }
        }
    }
}
