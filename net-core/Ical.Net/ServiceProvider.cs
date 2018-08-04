using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ical.Net
{
    public class ServiceProvider
    {
        private readonly IDictionary<string, object> _namedServices = new Dictionary<string, object>();
        private readonly IDictionary<Type, object> _typedServices = new Dictionary<Type, object>();

        public virtual object GetService(Type serviceType)
        {
            _typedServices.TryGetValue(serviceType, out object service);
            return service;
        }

        public virtual object GetService(string name)
        {
            _namedServices.TryGetValue(name, out object service);
            return service;
        }

        public virtual T GetService<T>()
        {
            var service = GetService(typeof(T));
            if (service is T)
            {
                return (T)service;
            }
            return default(T);
        }

        public virtual T GetService<T>(string name)
        {
            var service = GetService(name);
            if (service is T)
            {
                return (T)service;
            }
            return default(T);
        }

        public virtual void RemoveService(Type type)
        {
            if (type != null)
            {
                if (_typedServices.ContainsKey(type))
                {
                    _typedServices.Remove(type);
                }

                // Get interfaces for the given type
                foreach (var iface in type.GetInterfaces().Where(iface => _typedServices.ContainsKey(iface)))
                {
                    _typedServices.Remove(iface);
                }
            }
        }

        public virtual void RemoveService(string name)
        {
            if (_namedServices.ContainsKey(name))
            {
                _namedServices.Remove(name);
            }
        }

        public virtual void SetService(string name, object obj)
        {
            if (!string.IsNullOrEmpty(name) && obj != null)
            {
                _namedServices[name] = obj;
            }
        }

        public virtual void SetService(object obj)
        {
            if (obj != null)
            {
                var type = obj.GetType();
                _typedServices[type] = obj;

                // Get interfaces for the given type
                foreach (var iface in type.GetInterfaces())
                {
                    _typedServices[iface] = obj;
                }
            }
        }
    }
}