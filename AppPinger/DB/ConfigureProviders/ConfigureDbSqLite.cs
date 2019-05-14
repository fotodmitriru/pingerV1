using System;
using Microsoft.EntityFrameworkCore;

namespace AppPinger.DB.ConfigureProviders
{
    internal sealed class ConfigureDbSqLite : DbContext, IConfigureDb
    {
        private readonly string _dbConnectionString;
        public DbSet<LogModel> LogsModel { get; set; }

        public ConfigureDbSqLite(string dbConnectionString)
        {
            if (string.IsNullOrEmpty(dbConnectionString))
                throw new ArgumentNullException(dbConnectionString);
            _dbConnectionString = dbConnectionString;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_dbConnectionString);
        }
    }
}
