using System;

namespace Ical.Net
{
    public interface ITypedServicesProvider
    {
        object GetService(Type type);
        T GetService<T>();
        void SetService(object obj);
        void RemoveService(Type type);
    }

    public interface INamedServicesProvider
    {
        object GetService(string name);
        T GetService<T>(string name);
        void SetService(string name, object obj);
        void RemoveService(string name);
    }
}