using ConfigurationReaderLib.Entities;
using ConfigurationReaderLib.Helpers;
using ConfigurationReaderLib.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationReaderLib
{
    public class ConfigurationReader : IDisposable
    {
        private IConfigurationRepository _configurationRepository;
        private string _applicationName;
        private int _refreshTimerIntervalInMs;
        private List<Configuration> _configurationCache = new List<Configuration>();
        private TimerScheduler _timerScheduler;

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs, IConfigurationRepository configurationRepository = null)
        {
            if (configurationRepository == null)
            {
                _configurationRepository = new ConfigurationRepository(connectionString);
            }
            else
            {
                _configurationRepository = configurationRepository;
            }
            _timerScheduler = new TimerScheduler();
            _applicationName = applicationName;
            _refreshTimerIntervalInMs = refreshTimerIntervalInMs;

            LoadAndUpdateConfigurations();
        }

        private void LoadConfigurations()
        {
            var configurations = _configurationRepository.GetByApplicationName(_applicationName);

            if (configurations != null && configurations.Count > 0)
            {
                _configurationCache = configurations.Where(x => x.IsActive).ToList();
            }
        }

        private void LoadAndUpdateConfigurations()
        {
            LoadConfigurations();
            _timerScheduler.ScheduleTimer(LoadConfigurations, _refreshTimerIntervalInMs);
        }

        public T GetValue<T>(string name)
        {
            var configuration = _configurationCache.FirstOrDefault(x => x.Name == name);
            if (configuration == null)
            {
                throw new Exception("Configuration Value Not Found");
            }

            return (T)Convert.ChangeType(configuration.Value, typeof(T));
        }

        public List<Configuration> GetAll()
        {
            return _configurationCache.ToList();
        }

        public Configuration GetById(int id)
        {
            return _configurationCache.FirstOrDefault(x => x.ID == id);
        }

        public void SetConfiguration(string name, string value)
        {
            var configurationInCache = _configurationCache.FirstOrDefault(x => x.Name == name);
            if (configurationInCache != null)
            {
                configurationInCache.Value = value;
                _configurationRepository.Update(configurationInCache);
            }
            else
            {
                throw new Exception($"Configuration By Name {name} Not Found");
            }
        }

        public void UpdateConfiguration(Configuration configuration)
        {
            var isConfigurationExist = _configurationCache.Any(x => x.Name == configuration.Name);
            if (isConfigurationExist)
            {
                _configurationRepository.Update(configuration);
            }
            else
            {
                AddConfiguration(configuration);
            }
        }

        public void AddConfiguration(Configuration configuration)
        {
            _configurationRepository.Add(configuration);
        }

        public void DeleteConfiguration(Configuration configuration)
        {
            _configurationRepository.Delete(configuration);
        }

        public void Dispose()
        {
            _timerScheduler.Dispose();
        }
    }
}
