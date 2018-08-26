using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Ical.Net.CoreUnitTests
{
    [TestFixture]
    public class TypedServiceProviderTest
    {
        [Test]
        public void GetServiceShouldRetunNullWhenProviderEmpty()
        {
            // Arrange
            var provider = new TypedServiceProvider();

            // Act
            var actual = provider.GetService(typeof(int));

            // Assert  
            Assert.IsNull(actual);
        }

        [Test]
        public void GetServiceShouldRetunNullWhenTypeNotFound()
        {
            // Arrange
            var provider = new TypedServiceProvider();
            provider.SetService("STRING_TYPE");

            // Act
            var actual = provider.GetService(typeof(int));

            // Assert  
            Assert.IsNull(actual);
        }

        [Test]
        public void GetServiceShouldRetunObjectWhenTypeFound()
        {
            // Arrange
            var provider = new TypedServiceProvider();
            provider.SetService("STRING_TYPE");

            // Act
            var actual = provider.GetService(typeof(string));

            // Assert  
            Assert.AreEqual("STRING_TYPE", actual);
        }

        [Test]
        public void GetServiceShouldRetunLatestObjectOfSpecificType()
        {
            // Arrange
            var provider = new TypedServiceProvider();
            provider.SetService("FIRST_STRING_TYPE");
            provider.SetService("SECOND_STRING_TYPE");

            // Act
            var actual = provider.GetService(typeof(string));

            // Assert  
            Assert.AreEqual("SECOND_STRING_TYPE", actual);
        }

        [Test]
        public void SetServiceShouldIgnoreNullObjects()
        {
            // Arrange
            var provider = new TypedServiceProvider();
            provider.SetService((string)null);

            // Act
            var actual = provider.GetService(typeof(string));

            // Assert  
            Assert.IsNull(actual);
        }

        [Test]
        public void SetServiceShouldStoresInterfacesAlongsideClassType()
        {
            // Arrange
            var provider = new TypedServiceProvider();
            provider.SetService(new DummyClass());

            // Act
            var actual = provider.GetService(typeof(IDummyInterface));

            // Assert  
            Assert.AreEqual("DUMMY_CLASS", ((IDummyInterface)actual).Name);
        }

        [Test]
        public void RemoveServiceShouldNotThrowIfTypeNotFound()
        {
            // Arrange
            var provider = new TypedServiceProvider();

            // Act + Assert
            Assert.DoesNotThrow(() => provider.RemoveService(typeof(string)));
        }

        [Test]
        public void RemoveServiceShouldRemoveItemWhenFound()
        {
            // Arrange
            var provider = new TypedServiceProvider();
            provider.SetService("STRING_TYPE");

            // Act
            provider.RemoveService(typeof(string));
            var actual = provider.GetService(typeof(string));

            // Assert  
            Assert.IsNull(actual);
        }

        [Test]
        public void RemoveServiceShouldRemoveInterfaces()
        {
            // Arrange
            var provider = new TypedServiceProvider();
            provider.SetService(new DummyClass());

            // Act
            provider.RemoveService(typeof(DummyClass));
            var actual = provider.GetService(typeof(IDummyInterface));

            // Assert  
            Assert.IsNull(actual, $"Interface '{nameof(IDummyInterface)}' should not be present.");
        }

        private class DummyClass : IDummyInterface
        {
            public string Name => "DUMMY_CLASS";
        }

        private interface IDummyInterface
        {
            string Name { get; }
        }
    }
}
