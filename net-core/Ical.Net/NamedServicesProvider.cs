using System.Collections.Generic;

namespace Ical.Net
{
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