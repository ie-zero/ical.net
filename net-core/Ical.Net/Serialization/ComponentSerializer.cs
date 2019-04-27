using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ical.Net.CalendarComponents;
using Ical.Net.Utility;

namespace Ical.Net.Serialization
{
    public class ComponentSerializer : SerializerBase
    {
        protected virtual IComparer<ICalendarProperty> PropertySorter => new PropertyAlphabetizer();

        public ComponentSerializer(SerializationContext ctx) : base(ctx) { }

        public override Type TargetType => typeof(CalendarComponent);

        public override string SerializeToString(object obj)
        {
            if (!(obj is ICalendarComponent c))
            {
                return null;
            }

            // Get a serializer factory
            var factory = SerializationContext.GetService<ISerializerFactory>();

            // Sort the calendar properties in alphabetical order before serializing them!
            var properties = c.Properties.OrderBy(p => p.Name).ToArray();

            var builder = new StringBuilder();
            SerializeBeginComponent(builder, c.Name);
            SerializeProperties(builder, properties, factory);
            SerializeChildObjects(builder, c.Children, factory);
            SerializeEndComponent(builder, c.Name);

            return builder.ToString();
        }

        private static void SerializeBeginComponent(StringBuilder builder, string componentName)
        {
            string upperName = componentName.ToUpperInvariant();
            builder.Append(TextUtil.FoldLines($"BEGIN:{upperName}"));
        }

        private static void SerializeEndComponent(StringBuilder builder, string componentName)
        {
            string upperName = componentName.ToUpperInvariant();
            builder.Append(TextUtil.FoldLines($"END:{upperName}"));
        }

        private void SerializeProperties(StringBuilder builder, ICalendarProperty[] properties, ISerializerFactory factory)
        {
            foreach (var prop in properties)
            {
                // Get a serializer for each property.
                var serializer = factory.Build(prop.GetType(), SerializationContext) as IStringSerializer;
                builder.Append(serializer.SerializeToString(prop));
            }
        }

        private void SerializeChildObjects(StringBuilder builder, ICalendarObjectList<ICalendarObject> children, ISerializerFactory factory)
        {
            foreach (var child in children)
            {
                // Get a serializer for each child object.
                var serializer = factory.Build(child.GetType(), SerializationContext) as IStringSerializer;
                builder.Append(serializer.SerializeToString(child));
            }
        }

        public override object Deserialize(TextReader reader) => null;

        public class PropertyAlphabetizer : IComparer<ICalendarProperty>
        {
            public int Compare(ICalendarProperty x, ICalendarProperty y)
            {
                if (x == y)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                return y == null
                    ? 1
                    : string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}