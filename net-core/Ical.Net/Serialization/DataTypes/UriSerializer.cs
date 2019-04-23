using System;
using System.IO;
using Ical.Net.DataTypes;

namespace Ical.Net.Serialization.DataTypes
{
    public class UriSerializer : EncodableDataTypeSerializer
    {
        public UriSerializer() {}

        public UriSerializer(SerializationContext ctx) : base(ctx) {}

        public override Type TargetType => typeof(string);

        public override string SerializeToString(object obj)
        {
            var uri = obj as Uri;
            if (uri == null) return null;

            if (SerializationContext.Peek() is ICalendarObject co)
            {
                var dt = new EncodableDataType
                {
                    AssociatedObject = co
                };
                return Encode(dt, uri.OriginalString);
            }
            return uri.OriginalString;
        }

        public override object Deserialize(TextReader reader)
        {
            if (reader == null) return null;

            var value = reader.ReadToEnd();

            if (SerializationContext.Peek() is ICalendarObject co)
            {
                var dt = new EncodableDataType
                {
                    AssociatedObject = co
                };
                value = Decode(dt, value);
            }

            try
            {
                return new Uri(value);
            }
            // TODO: Review code - exceptions are swallowed silently
            catch { }
            return null;
        }
    }
}