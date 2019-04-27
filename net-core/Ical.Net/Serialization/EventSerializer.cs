using System;
using Ical.Net.CalendarComponents;

namespace Ical.Net.Serialization
{
    public class EventSerializer : ComponentSerializer
    {
        public EventSerializer(SerializationContext ctx) : base(ctx) { }

        public override Type TargetType => typeof(CalendarEvent);

        public override string SerializeToString(object obj)
        {
            var evt = obj as CalendarEvent;
            if (evt == null) return null;

            CalendarEvent actualEvent = RemoveDurationIfNecessary(evt);
            return base.SerializeToString(actualEvent);
        }

        private static CalendarEvent RemoveDurationIfNecessary(CalendarEvent evt)
        {
            if (evt.Properties.ContainsKey("DURATION") && evt.Properties.ContainsKey("DTEND"))
            {
                CalendarEvent actualEvent = evt.Copy<CalendarEvent>();
                actualEvent.Properties.Remove("DURATION");

                return actualEvent;
            }
            else
            {
                return evt;
            }
        }
    }
}