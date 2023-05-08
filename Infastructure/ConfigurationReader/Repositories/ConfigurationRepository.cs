using ConfigurationReaderLib.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ConfigurationReaderLib.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        public ConfigurationContext _configurationContext;
        private readonly string _connectionString;

        public ConfigurationRepository(string connectionString)
        {
            _connectionString = connectionString;
            _configurationContext = new ConfigurationContext(_connectionString);
            InitDatabase();
        }

        public void Add(Configuration configuration)
        {
            _configurationContext.Configurations.Add(configuration);
            _configurationContext.SaveChanges();
        }

        public Configuration? GetById(int id)
        {
            return _configurationContext.Configurations.FirstOrDefault(x => x.ID == id);
        }

        public Configuration? GetByName(string name)
        {
            return _configurationContext.Configurations.FirstOrDefault(x => x.Name == name);
        }

        public List<Configuration> GetAll()
        {
            return _configurationContext.Configurations.ToList();
        }

        public void Update(Configuration configuration)
        {
            _configurationContext.Update(configuration);
            _configurationContext.SaveChanges();
        }

        public void Delete(Configuration configuration)
        {
            _configurationContext.Remove(configuration);
            _configurationContext.SaveChanges();
        }

        public List<Configuration> GetByApplicationName(string applicationName)
        {
           
            return _configurationContext.Configurations.Where(x => x.ApplicationName == applicationName).ToList();
        }

        private void InitDatabase()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "DROP TABLE Configurations;";
                connection.Execute(sql);
            }
            // create database tables if they don't exist
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "CREATE TABLE Configurations (ID int,[Name] nvarchar(50) not null, [Type] nvarchar(50) not null,IsActive bit,ApplicationName nvarchar(50),Value nvarchar(50));";
                connection.Execute(sql);
            }

            Add(new Configuration()
            {
                ID = 1,
                ApplicationName = "ServiceA",
                IsActive = true,
                Name = "Test",
                Type = "string",
                Value = "Test Value"
            });

            Add(new Configuration()
            {
                ID = 2,
                ApplicationName = "ServiceA",
                IsActive = true,
                Name = "Test 2",
                Type = "string",
                Value = "Test Value 2"
            });
        }
    }
}
