using ConfigurationReaderLib;
using ConfigurationReaderLib.Entities;

namespace BeymenWebApp.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private ConfigurationReader _configurationReader;

        public ConfigurationService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("Database");
            _configurationReader = new ConfigurationReader("ServiceA", connectionString, 5000);
        }

        public void Add(Models.Configuration configuration)
        {
            var newConfiguration = ConvertToDatabaseModel(configuration);
            _configurationReader.AddConfiguration(newConfiguration);
        }

        public List<Models.Configuration> GetAllConfigurations()
        {
            return _configurationReader.GetAll().Select(x => ConvertToViewModel(x)).ToList();
        }

        public Models.Configuration GetById(int id)
        {
            var configuration = _configurationReader.GetById(id);

            if (configuration != null)
            {
                return ConvertToViewModel(configuration);
            }

            return null;
        }

        public void Update(Models.Configuration configuration)
        {
            var newConfiguration = ConvertToDatabaseModel(configuration);
            _configurationReader.UpdateConfiguration(newConfiguration);
        }

        public void Delete(Models.Configuration configuration)
        {
            var newConfiguration = ConvertToDatabaseModel(configuration);
            _configurationReader.DeleteConfiguration(newConfiguration);
        }

        private Configuration ConvertToDatabaseModel(Models.Configuration configuration)
        {
            return new Configuration()
            {
                ID = configuration.ID,
                ApplicationName = configuration.ApplicationName,
                IsActive = configuration.IsActive,
                Name = configuration.Name,
                Type = configuration.Type,
                Value = configuration.Value
            };
        }

        private Models.Configuration ConvertToViewModel(Configuration configuration)
        {
            return new Models.Configuration()
            {
                ID = configuration.ID,
                ApplicationName = configuration.ApplicationName,
                IsActive = configuration.IsActive,
                Name = configuration.Name,
                Type = configuration.Type,
                Value = configuration.Value
            };
        }
    }
}
