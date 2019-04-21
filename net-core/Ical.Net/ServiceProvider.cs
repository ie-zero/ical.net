using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ical.Net
{
    public class ServiceProvider
    {
        private readonly TypedServicesProvider _typedServices;
        private readonly NamedServicesProvider _namedServices;

        public ServiceProvider()
        {
            _typedServices = new TypedServicesProvider();
            _namedServices = new NamedServicesProvider();
        }

        public object GetService(Type serviceType)
        {
            return _typedServices.GetService(serviceType);
        }

        public object GetService(string name)
        {
            return _namedServices.GetService(name);
        }

        public T GetService<T>()
        {
            return _typedServices.GetService<T>();
        }

        public T GetService<T>(string name)
        {
            return _namedServices.GetService<T>(name);
        }

        public void SetService(string name, object obj)
        {
            _namedServices.SetService(name, obj);
        }

        public void SetService(object obj)
        {
            _typedServices.SetService(obj);
        }

        public void RemoveService(Type type)
        {
            _typedServices.RemoveService(type);
        }

        public void RemoveService(string name)
        {
            _namedServices.RemoveService(name);
        }
    }

    public class TypedServicesProvider
    {
        private readonly IDictionary<Type, object> _services = new Dictionary<Type, object>();

        public object GetService(Type serviceType)
        {
            _services.TryGetValue(serviceType, out object service);
            return service;
        }

        public T GetService<T>()
        {
            var service = GetService(typeof(T));
            if (service is T)
            {
                return (T)service;
            }
            return default(T);
        }

        /// <summary>
        /// Stores a unique entry for the type and each interface of the supplied object. If multiple objects 
        /// implement the same interface only the last one added will be retrievable under the interface type.
        /// </summary>
        public void SetService(object obj)
        {
            if (obj == null) { return; }

            var type = obj.GetType();
            _services[type] = obj;

            // Get interfaces for the given type
            foreach (var iface in type.GetInterfaces())
            {
                _services[iface] = obj;
            }
        }

        public void RemoveService(Type type)
        {
            if (type != null)
            {
                if (_services.ContainsKey(type))
                {
                    _services.Remove(type);
                }

                // TODO: Validate that the wrong type cannot be removed by mistake if multiple classes are implementing the same interface.

                // Get interfaces for the given type
                foreach (var iface in type.GetInterfaces().Where(iface => _services.ContainsKey(iface)))
                {
                    _services.Remove(iface);
                }
            }
        }
    }


    public class NamedServicesProvider
    {
        private readonly IDictionary<string, object> _services = new Dictionary<string, object>();

        public object GetService(string name)
        {
            object service;
            _services.TryGetValue(name, out service);
            return service;
        }

        public T GetService<T>(string name)
        {
            var service = GetService(name);
            if (service is T)
            {
                return (T)service;
            }
            return default(T);
        }

        public void SetService(string name, object obj)
        {
            if (!string.IsNullOrEmpty(name) && obj != null)
            {
                _services[name] = obj;
            }
        }

        public void RemoveService(string name)
        {
            if (_services.ContainsKey(name))
            {
                _services.Remove(name);
            }
        }
    }
}