using System;
using System.Reflection;
using Ical.Net.DataTypes;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.Serialization
{
    public class DataTypeSerializerFactory : ISerializerFactory
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
        public ISerializer Build(Type objectType, SerializationContext ctx)
        {
            // TODO: Consider if returning null for ISerializer is logical.
            if (objectType == null) { return null; }

            if (typeof(Attachment).IsAssignableFrom(objectType))
            {
                return new AttachmentSerializer(ctx);
            }
            else if (typeof(Attendee).IsAssignableFrom(objectType))
            {
                return new AttendeeSerializer(ctx);
            }
            else if (typeof(IDateTime).IsAssignableFrom(objectType))
            {
                return new DateTimeSerializer(ctx);
            }
            else if (typeof(FreeBusyEntry).IsAssignableFrom(objectType))
            {
                return new FreeBusyEntrySerializer(ctx);
            }
            else if (typeof(GeographicLocation).IsAssignableFrom(objectType))
            {
                return new GeographicLocationSerializer(ctx);
            }
            else if (typeof(Organizer).IsAssignableFrom(objectType))
            {
                return new OrganizerSerializer(ctx);
            }
            else if (typeof(Period).IsAssignableFrom(objectType))
            {
                return new PeriodSerializer(ctx);
            }
            else if (typeof(PeriodList).IsAssignableFrom(objectType))
            {
                return new PeriodListSerializer(ctx);
            }
            else if (typeof(RecurrencePattern).IsAssignableFrom(objectType))
            {
                return new RecurrencePatternSerializer(ctx);
            }
            else if (typeof(RequestStatus).IsAssignableFrom(objectType))
            {
                return new RequestStatusSerializer(ctx);
            }
            else if (typeof(StatusCode).IsAssignableFrom(objectType))
            {
                return new StatusCodeSerializer(ctx);
            }
            else if (typeof(Trigger).IsAssignableFrom(objectType))
            {
                return new TriggerSerializer(ctx);
            }
            else if (typeof(UtcOffset).IsAssignableFrom(objectType))
            {
                return new UtcOffsetSerializer(ctx);
            }
            else if (typeof(WeekDay).IsAssignableFrom(objectType))
            {
                return new WeekDaySerializer(ctx);
            }

            // Default to a string serializer, which simply calls ToString() on the value to serialize it.
            return new StringSerializer(ctx);
        }
    }
}