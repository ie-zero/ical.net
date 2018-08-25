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
        private ParameterList _parameters;
        private ServiceProvider _serviceProvider;

        protected CalendarDataType()
        {
            Initialize();
        }

        public ICalendarObject AssociatedObject
        {
            get { return _associatedObject; }
            internal set { Associate(value); }
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

        // TODO: Consider merging Associate() method with AssociatedObject property setter.
        //      The Associate() method is used as the external API of the class where the AssociatedObject 
        //      setter for internal calls. 
        public void Associate(ICalendarObject associatedObject)
        {
            if (Equals(_associatedObject, associatedObject)) { return; }

            _associatedObject = associatedObject;
            if (_associatedObject != null)
            {
                _parameters.SetParent(_associatedObject);

                var parameterContainer = _associatedObject as IPropertyParameters;
                if (parameterContainer != null)
                {
                    _parameters.Clear();
                    foreach (var param in parameterContainer.Parameters)
                    {
                        _parameters.Add(param);
                    }
                }
            }
            else
            {
                _parameters.SetParent(null);
            }
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
        /// Copies values from the input object to the current object.
        /// </summary>
        public virtual void CopyFrom(ICopyable copyable)
        {
            var dataType = copyable as ICalendarDataType;
            if (dataType == null) { return; }

            CopyFrom(dataType);
        }

        private void CopyFrom(ICalendarDataType dataType)
        {
            _associatedObject = dataType.AssociatedObject;
            _parameters.SetParent(_associatedObject);
            _parameters.Clear();
            foreach (var param in dataType.Parameters)
            {
                _parameters.Add(param);
            }
        }

        internal object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        internal T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        internal void SetService(object obj)
        {
            _serviceProvider.SetService(obj);
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
            _parameters = new ParameterList();
            _serviceProvider = new ServiceProvider();
        }
    }
}