using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationReaderLib.Entities
{
    public class Configuration
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; }
        public string Value { get; set; }
    }
}
