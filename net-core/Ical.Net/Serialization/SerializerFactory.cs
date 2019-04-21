using System;
using System.Reflection;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.Serialization
{
    public class SerializerFactory : ISerializerFactory
    {
        /// <summary>
        /// Returns a serializer that can be used to serialize and object
        /// of type <paramref name="objectType"/>.
        /// <note>
        ///     TODO: Add support for caching.
        /// </note>
        /// </summary>
        /// <param name="objectType">The type of object to be serialized.</param>
        /// <param name="ctx">The serialization context.</param>
        public virtual ISerializer Build(Type objectType, SerializationContext ctx)
        {
            if (objectType == null) { return null; }

            if (typeof(Calendar).IsAssignableFrom(objectType))
            {
                return new CalendarSerializer(ctx);
            }
            else if (typeof(ICalendarComponent).IsAssignableFrom(objectType))
            {
                return typeof(CalendarEvent).IsAssignableFrom(objectType)
                    ? new EventSerializer(ctx)
                    : new ComponentSerializer(ctx);
            }
            else if (typeof(ICalendarProperty).IsAssignableFrom(objectType))
            {
                return new PropertySerializer(ctx);
            }
            else if (typeof(CalendarParameter).IsAssignableFrom(objectType))
            {
                return new ParameterSerializer(ctx);
            }
            else if (typeof(string).IsAssignableFrom(objectType))
            {
                return new StringSerializer(ctx);
            }
            else if (objectType.GetTypeInfo().IsEnum)
            {
                return new EnumSerializer(objectType, ctx);
            }
            else if (typeof(TimeSpan).IsAssignableFrom(objectType))
            {
                return new TimeSpanSerializer(ctx);
            }
            else if (typeof(int).IsAssignableFrom(objectType))
            {
                return new IntegerSerializer(ctx);
            }
            else if (typeof(Uri).IsAssignableFrom(objectType))
            {
                return new UriSerializer(ctx);
            }
            else if (typeof(ICalendarDataType).IsAssignableFrom(objectType))
            {
                return new DataTypeSerializerFactory().Build(objectType, ctx);
            }

            // Default to a string serializer, which simply calls
            // ToString() on the value to serialize it.
            return new StringSerializer(ctx);
        }
    }
}