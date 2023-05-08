using ConfigurationReaderLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationReaderLib.Repositories
{
    public interface IConfigurationRepository
    {
        List<Configuration> GetAll();

        List<Configuration> GetByApplicationName(string applicationName);

        Configuration? GetById(int id);

        Configuration? GetByName(string name);

        void Add(Configuration configuration);

        void Update(Configuration configuration);
        void Delete(Configuration configuration);

    }
}
