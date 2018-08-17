using System;
using System.Runtime.Serialization;
using Ical.Net.Proxies;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// An abstract class from which all iCalendar data types inherit.
    /// </summary>
    public abstract class CalendarDataType : ICalendarDataType, ICopyable
    {
        protected ICalendarObject _associatedObject;
        private ParameterCollectionProxy _parameters;
        private ServiceProvider _serviceProvider;

        protected CalendarDataType()
        {
            Initialize();
        }

        public virtual ICalendarObject AssociatedObject
        {
            get { return _associatedObject; }

            set
            {
                if (Equals(_associatedObject, value)) { return; }

                _associatedObject = value;
                if (_associatedObject != null)
                {
                    _parameters.SetParent(_associatedObject);

                    var parameterContainer = _associatedObject as ICalendarParameterCollectionContainer;
                    if (parameterContainer != null)
                    {
                        _parameters.SetProxiedObject(parameterContainer.Parameters);
                    }
                }
                else
                {
                    _parameters.SetParent(null);
                }
            }
        }

        public Calendar Calendar
        {
            get { return _associatedObject?.Calendar; }
        }

        public string Language
        {
            get => Parameters.Get("LANGUAGE");
            set => Parameters.Set("LANGUAGE", value);
        }

        public IParameterCollection Parameters => _parameters;

        public void Associate(ICalendarObject associatedObject)
        {
            if (associatedObject == null) { return; }
            AssociatedObject = associatedObject;            
        }

        /// <summary>
        /// Creates a copy of the object.
        /// </summary>
        /// <returns>The copy of the object.</returns>
        public T Copy<T>()
        {
            var type = GetType();
            var obj = Activator.CreateInstance(type) as ICopyable;

            // Duplicate our values
            if (obj is T)
            {
                obj.CopyFrom(this);
                return (T)obj;
            }
            return default(T);
        }

        /// <summary>
        /// Copies values from the target object to the
        /// current object.
        /// </summary>
        public virtual void CopyFrom(ICopyable obj)
        {
            if (!(obj is ICalendarDataType))
            {
                return;
            }

            var dt = (ICalendarDataType)obj;
            _associatedObject = dt.AssociatedObject;
            _parameters.SetParent(_associatedObject);
            _parameters.SetProxiedObject(dt.Parameters);
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public object GetService(string name)
        {
            return _serviceProvider.GetService(name);
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public T GetService<T>(string name)
        {
            return _serviceProvider.GetService<T>(name);
        }

        public Type GetValueType()
        {
            // See RFC 5545 Section 3.2.20.
            if (_parameters != null && _parameters.ContainsKey("VALUE"))
            {
                switch (_parameters.Get("VALUE"))
                {
                    case "BINARY":
                        return typeof(byte[]);
                    case "BOOLEAN":
                        return typeof(bool);
                    case "CAL-ADDRESS":
                        return typeof(Uri);
                    case "DATE":
                        return typeof(IDateTime);
                    case "DATE-TIME":
                        return typeof(IDateTime);
                    case "DURATION":
                        return typeof(TimeSpan);
                    case "FLOAT":
                        return typeof(double);
                    case "INTEGER":
                        return typeof(int);
                    case "PERIOD":
                        return typeof(Period);
                    case "RECUR":
                        return typeof(RecurrencePattern);
                    case "TEXT":
                        return typeof(string);
                    case "TIME":
                        // TODO: FIX - Not implemented (ISO.8601.2004)
                        throw new NotImplementedException();
                    case "URI":
                        return typeof(Uri);
                    case "UTC-OFFSET":
                        return typeof(UtcOffset);
                    default:
                        return null;
                }
            }
            return null;
        }

        public void RemoveService(Type type)
        {
            _serviceProvider.RemoveService(type);
        }

        public void RemoveService(string name)
        {
            _serviceProvider.RemoveService(name);
        }

        public void SetService(string name, object obj)
        {
            _serviceProvider.SetService(name, obj);
        }

        public void SetService(object obj)
        {
            _serviceProvider.SetService(obj);
        }

        public void SetValueType(string type)
        {
            _parameters?.Set("VALUE", type ?? type.ToUpper());
        }

        [OnDeserialized]
        internal void DeserializedInternal(StreamingContext context)
        {
            OnDeserialized(context);
        }

        [OnDeserializing]
        internal void DeserializingInternal(StreamingContext context)
        {
            OnDeserializing(context);
        }

        protected void OnDeserialized(StreamingContext context) { }

        protected void OnDeserializing(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            _parameters = new ParameterCollectionProxy(new ParameterList());
            _serviceProvider = new ServiceProvider();
        }
    }
}