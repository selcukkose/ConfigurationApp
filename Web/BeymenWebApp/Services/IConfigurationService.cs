using ConfigurationReaderLib.Entities;

namespace BeymenWebApp.Services
{
    public interface IConfigurationService
    {
        List<Models.Configuration> GetAllConfigurations();
        Models.Configuration GetById(int id);

        void Add(Models.Configuration configuration);

        void Update(Models.Configuration configuration);

        void Delete(Models.Configuration configuration);
    }
}
