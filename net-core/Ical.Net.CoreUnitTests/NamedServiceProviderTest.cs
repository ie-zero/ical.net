using NUnit.Framework;

namespace Ical.Net.CoreUnitTests
{
    [TestFixture]
    public class NamedServiceProviderTest
    {
        [Test]
        public void GetServiceShouldRetunNullWhenProviderEmpty()
        {
            // Arrange
            var provider = new NamedServiceProvider();

            // Act
            var actual = provider.GetService("NAME_NOT_FOUND");

            // Assert  
            Assert.IsNull(actual);
        }

        [Test]
        public void GetServiceShouldRetunNullWhenNameNotFound()
        {
            // Arrange
            var provider = new NamedServiceProvider();
            provider.SetService("SERVICE_NAME", "SERVICE_VALUE");

            // Act
            var actual = provider.GetService("ANOTHER_NAME");

            // Assert  
            Assert.IsNull(actual);
        }

        [Test]
        public void GetServiceShouldRetunObjectWhenNameExists()
        {
            // Arrange
            var provider = new NamedServiceProvider();
            provider.SetService("SERVICE_NAME", "SERVICE_VALUE");

            // Act
            var actual = provider.GetService("SERVICE_NAME");

            // Assert  
            Assert.AreEqual("SERVICE_VALUE", actual);
        }

        [Test]
        public void GetServiceShouldRetunLatestObjectOfSpecificName()
        {
            // Arrange
            var provider = new NamedServiceProvider();
            provider.SetService("SERVICE_NAME", "FIRST_VALUE");
            provider.SetService("SERVICE_NAME", "SECOND_VALUE");

            // Act
            var actual = provider.GetService("SERVICE_NAME");

            // Assert  
            Assert.AreEqual("SECOND_VALUE", actual);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]        
        public void SetServiceShouldIgnoreInvalidNames(string name)
        {
            // Arrange
            var provider = new NamedServiceProvider();

            // Act
            provider.SetService(name, "SERVICE_VALUE");
            var actual = provider.GetService(name);

            // Assert  
            Assert.IsNull(actual);
        }

        [Test]
        public void RemoveServiceShouldNotThrowIfNameNotFound()
        {
            // Arrange
            var provider = new NamedServiceProvider();

            // Act + Assert
            Assert.DoesNotThrow(() => provider.RemoveService("SERVICE_NAME"));
        }

        [Test]
        public void RemoveServiceShouldRemoveItemWhenFound()
        {
            // Arrange
            var provider = new NamedServiceProvider();
            provider.SetService("SERVICE_NAME", "SERVICE_VALUE");

            // Act
            provider.RemoveService("SERVICE_NAME");
            var actual = provider.GetService("SERVICE_NAME");

            // Assert  
            Assert.IsNull(actual);
        }
    }
}
