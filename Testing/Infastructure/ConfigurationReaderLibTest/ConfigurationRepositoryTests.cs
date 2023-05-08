using ConfigurationReaderLib;
using ConfigurationReaderLib.Entities;
using ConfigurationReaderLib.Repositories;
using Moq;

namespace ConfigurationReaderLibTest
{
    public class ConfigurationRepositoryTests
    {
        private Mock<IConfigurationRepository> _configurationRepositoryMock;
        private ConfigurationReader _sut;
        private List<Configuration> _configurationListTestData = new List<Configuration>
            {
                new Configuration()
                {
                    ID = 1,
                    ApplicationName = "ServiceA",
                    IsActive = true,
                    Name= "Test",
                    Type = "string",
                    Value = "Test Value"
                },
                new Configuration()
                {
                    ID = 1,
                    ApplicationName = "ServiceA",
                    IsActive = false,
                    Name= "Test2",
                    Type = "string",
                    Value = "Another Test Value"
                },
                 new Configuration()
                {
                    ID = 1,
                    ApplicationName = "ServiceA",
                    IsActive = true,
                    Name= "Test3",
                    Type = "int",
                    Value = "55"
                },
                   new Configuration()
                {
                    ID = 1,
                    ApplicationName = "ServiceB",
                    IsActive = true,
                    Name= "Test4",
                    Type = "int",
                    Value = "678"
                },
                    new Configuration()
                {
                    ID = 1,
                    ApplicationName = "ServiceB",
                    IsActive = true,
                    Name= "Test",
                    Type = "string",
                    Value = "Another Test Value Too"
                }
            };


        [SetUp]
        public void Setup()
        {
            _configurationRepositoryMock= new Mock<IConfigurationRepository>();
            _configurationRepositoryMock.Setup(x => x.GetByApplicationName(It.IsAny<string>()))
                .Returns((string applicationName) =>_configurationListTestData.Where(x => x.ApplicationName == applicationName).ToList());
            _configurationRepositoryMock.Setup(x => x.Update(It.IsAny<Configuration>()))
               .Callback((Configuration configuration) => {
                   var existingConfig = _configurationListTestData.FirstOrDefault(x => x.Name == configuration.Name);
                   existingConfig.Value = configuration.Value;
               });
        }

        [Test(Description = "GetConfiguration should return string configuration value by name")]
        public void GetConfiguration_ShouldReturn_StringConfigurationValueByName()
        {
            // Arrange
            _sut = new ConfigurationReader("ServiceA", @"Server=EPTRANKW024D\SQLEXPRESS;Database=configurationdb;Trusted_Connection=True", 5000, _configurationRepositoryMock.Object);

            // Act
            var configurationValue = _sut.GetValue<string>("Test");

            // Assert
            var expectedConfiguration = _configurationListTestData.FirstOrDefault(x => x.Name == "Test" && x.ApplicationName =="ServiceA");
            Assert.IsNotNull(configurationValue);
            Assert.AreEqual(expectedConfiguration.Value, configurationValue);
        }

        [Test(Description = "GetConfiguration should set configuration value")]
        public void GetConfiguration_Should_SetConfigurationValue()
        {
            // Arrange
            _sut = new ConfigurationReader("ServiceA", @"Server=EPTRANKW024D\SQLEXPRESS;Database=configurationdb;Trusted_Connection=True", 5000, _configurationRepositoryMock.Object);
            var newValue = "My Test Value";

            // Act
            _sut.SetConfiguration("Test", newValue);

            // Assert
            var expectedConfiguration = _configurationListTestData.FirstOrDefault(x => x.Name == "Test" && x.ApplicationName == "ServiceA");
            Assert.AreEqual(expectedConfiguration.Value, newValue);
        }
    }
}