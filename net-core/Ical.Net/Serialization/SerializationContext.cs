using System;
using System.Collections.Generic;

namespace Ical.Net.Serialization
{
    public class SerializationContext
    {
        private static SerializationContext _default;

        /// <summary>
        /// Gets the Singleton instance of the SerializationContext class.
        /// </summary>
        public static SerializationContext Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new SerializationContext();
                }

                // Create a new serialization context that doesn't contain any objects
                // (and is non-static).  That way, if any objects get pushed onto
                // the serialization stack when the Default serialization context is used,
                // and something goes wrong and the objects don't get popped off the stack,
                // we don't need to worry (as much) about a memory leak, because the
                // objects weren't pushed onto a stack referenced by a static variable.
                var ctx = new SerializationContext
                {
                    _typedServices = _default._typedServices,
                    _namedServices = _default._namedServices
                };
                return ctx;
            }
        }

        private readonly Stack<WeakReference> _mStack = new Stack<WeakReference>();
        private TypedServicesProvider _typedServices;
        private NamedServicesProvider _namedServices;

        public SerializationContext()
        {
            _typedServices = new TypedServicesProvider();
            _namedServices = new NamedServicesProvider();

            // Add some services by default
            SetService(new SerializerFactory());
            SetService(new CalendarComponentFactory());
            SetService(new DataTypeMapper());
            SetService(new EncodingStack());
            SetService(new EncodingProvider(this));
        }

        public virtual void Push(object item)
        {
            if (item != null)
            {
                _mStack.Push(new WeakReference(item));
            }
        }

        public virtual object Pop()
        {
            if (_mStack.Count > 0)
            {
                var r = _mStack.Pop();
                if (r.IsAlive)
                {
                    return r.Target;
                }
            }
            return null;
        }

        public virtual object Peek()
        {
            if (_mStack.Count > 0)
            {
                var r = _mStack.Peek();
                if (r.IsAlive)
                {
                    return r.Target;
                }
            }
            return null;
        }

        public virtual object GetService(Type serviceType)
        {
            return _typedServices.GetService(serviceType);
        }

        public virtual object GetService(string name)
        {
            return _namedServices.GetService(name);
        }

        public virtual T GetService<T>()
        {
            return _typedServices.GetService<T>();
        }

        public virtual T GetService<T>(string name)
        {
            return _namedServices.GetService<T>(name);
        }

        public virtual void SetService(string name, object obj)
        {
            _namedServices.SetService(name, obj);
        }

        public void SetService(object obj)
        {
            _typedServices.SetService(obj);
        }

        public virtual void RemoveService(Type type)
        {
            _typedServices.RemoveService(type);
        }

        public virtual void RemoveService(string name)
        {
            _namedServices.RemoveService(name);
        }
    }
}