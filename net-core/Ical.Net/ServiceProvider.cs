using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ical.Net
{
    public class ServiceProvider
    {
        readonly NamedServiceProvider _namedServices;
        readonly TypedServiceProvider _typedServices;

        public ServiceProvider()
        {
            _namedServices = new NamedServiceProvider();
            _typedServices = new TypedServiceProvider();
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
            var service = GetService(typeof(T));
            if (service is T)
            {
                return (T)service;
            }
            return default(T);
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

        public void RemoveService(Type type)
        {
            _typedServices.RemoveService(type);
        }

        public void RemoveService(string name)
        {
            _namedServices.RemoveService(name);
        }

        public void SetService(string name, object obj)
        {
            _namedServices.SetService(name, obj);
        }

        public void SetService(object obj)
        {
            _typedServices.SetService(obj);
        }
    }

    public class NamedServiceProvider
    {
        readonly IDictionary<string, object> _services = new Dictionary<string, object>();

        public object GetService(string name)
        {
            _services.TryGetValue(name, out object service);
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
            if (string.IsNullOrWhiteSpace(name)) { return; }
            if (obj == null) { return; }

            _services[name] = obj;
        }

        public void RemoveService(string name)
        {
            if (_services.ContainsKey(name))
            {
                _services.Remove(name);
            }
        }
    }

    public class TypedServiceProvider
    {
        readonly IDictionary<Type, object> _services = new Dictionary<Type, object>();

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

                // Get interfaces for the given type
                foreach (var iface in type.GetInterfaces().Where(iface => _services.ContainsKey(iface)))
                {
                    _services.Remove(iface);
                }
            }
        }
    }
}