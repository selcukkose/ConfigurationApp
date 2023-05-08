using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationReaderLib.Entities
{
    public class ConfigurationContext : DbContext
    {
        private readonly string _connectionString;

        public ConfigurationContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Configuration> Configurations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString, builder =>
            {
                builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
            });
        }
    }
}
