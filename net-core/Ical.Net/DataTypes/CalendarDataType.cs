using System;
using System.Runtime.Serialization;
using Ical.Net.Proxies;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// An abstract class from which all iCalendar data types inherit.
    /// </summary>
    public abstract class CalendarDataType : ICalendarDataType
    {
        private IParameterCollection _parameters;
        private ParameterCollectionProxy _proxy;
        private TypedServicesProvider _typedServices;

        protected ICalendarObject _AssociatedObject;

        protected CalendarDataType()
        {
            Initialize();
        }

        private void Initialize()
        {
            _parameters = new ParameterList();
            _proxy = new ParameterCollectionProxy(_parameters);
            _typedServices = new TypedServicesProvider();
        }

        [OnDeserializing]
        internal void DeserializingInternal(StreamingContext context)
        {
            OnDeserializing(context);
        }

        [OnDeserialized]
        internal void DeserializedInternal(StreamingContext context)
        {
            OnDeserialized(context);
        }

        protected void OnDeserializing(StreamingContext context)
        {
            Initialize();
        }

        protected void OnDeserialized(StreamingContext context) {}

        public Type GetValueType()
        {
            // See RFC 5545 Section 3.2.20.
            if (_proxy != null && _proxy.ContainsKey("VALUE"))
            {
                switch (_proxy.Get("VALUE"))
                {
                    case "BINARY":
                        return typeof (byte[]);
                    case "BOOLEAN":
                        return typeof (bool);
                    case "CAL-ADDRESS":
                        return typeof (Uri);
                    case "DATE":
                        return typeof (CalDateTime);
                    case "DATE-TIME":
                        return typeof (CalDateTime);
                    case "DURATION":
                        return typeof (TimeSpan);
                    case "FLOAT":
                        return typeof (double);
                    case "INTEGER":
                        return typeof (int);
                    case "PERIOD":
                        return typeof (Period);
                    case "RECUR":
                        return typeof (RecurrencePattern);
                    case "TEXT":
                        return typeof (string);
                    case "TIME":
                        // FIXME: implement ISO.8601.2004
                        throw new NotImplementedException();
                    case "URI":
                        return typeof (Uri);
                    case "UTC-OFFSET":
                        return typeof (UtcOffset);
                    default:
                        return null;
                }
            }
            return null;
        }

        public void SetValueType(string type)
        {
            _proxy?.Set("VALUE", type ?? type.ToUpper());
        }

        public ICalendarObject AssociatedObject
        {
            get => _AssociatedObject;
            set
            {
                if (Equals(_AssociatedObject, value))
                {
                    return;
                }

                _AssociatedObject = value;
                if (_AssociatedObject != null)
                {
                    _proxy.SetParent(_AssociatedObject);
                    if (_AssociatedObject is ICalendarParameterCollectionContainer)
                    {
                        _proxy.SetProxiedObject(((ICalendarParameterCollectionContainer) _AssociatedObject).Parameters);
                    }
                }
                else
                {
                    _proxy.SetParent(null);
                    _proxy.SetProxiedObject(_parameters);
                }
            }
        }

        public Calendar Calendar => _AssociatedObject?.Calendar;

        public string Language
        {
            get => Parameters.Get("LANGUAGE");
            set => Parameters.Set("LANGUAGE", value);
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

            var dt = (ICalendarDataType) obj;
            _AssociatedObject = dt.AssociatedObject;
            _proxy.SetParent(_AssociatedObject);
            _proxy.SetProxiedObject(dt.Parameters);
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
                return (T) obj;
            }
            return default(T);
        }

        public IParameterCollection Parameters => _proxy;

        internal object GetService(Type serviceType)
        {
            return _typedServices.GetService(serviceType);
        }

        internal T GetService<T>()
        {
            return _typedServices.GetService<T>();
        }

        internal void SetService(object obj)
        {
            _typedServices.SetService(obj);
        }

        internal void RemoveService(Type type)
        {
            _typedServices.RemoveService(type);
        }
    }
}