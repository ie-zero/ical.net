using System;
using System.IO;
using System.Reflection;
using Ical.Net.DataTypes;

namespace Ical.Net.Serialization.DataTypes
{
    public class EnumSerializer : EncodableDataTypeSerializer
    {
        private readonly Type _enumType;

        public EnumSerializer(SerializationContext ctx, Type enumType) : base(ctx)
        {
            if (enumType == null)
                throw new ArgumentNullException(nameof(enumType));

            if (!enumType.GetTypeInfo().IsEnum)
                throw new ArgumentException($"'{nameof(enumType)}' is not an enumeration type", nameof(enumType));

            _enumType = enumType;
        }

        public override Type TargetType => _enumType;

        public override string SerializeToString(object enumValue)
        {
            if (enumValue == null) return null;

            try
            {
                var obj = SerializationContext.Peek() as ICalendarObject;
                if (obj != null)
                {
                    // Encode the value as needed.
                    var dt = new EncodableDataType
                    {
                        AssociatedObject = obj
                    };
                    return Encode(dt, enumValue.ToString());
                }
                return enumValue.ToString();
            }
            catch
            {
                // TODO: Review code - exceptions are swallowed silently
                return null;
            }
        }

        public override object Deserialize(TextReader reader)
        {
            if (reader == null) return null;

            string value = reader.ReadToEnd();

            try
            {
                var obj = SerializationContext.Peek() as ICalendarObject;
                if (obj != null)
                {
                    // Decode the value, if necessary!
                    var dt = new EncodableDataType
                    {
                        AssociatedObject = obj
                    };
                    value = Decode(dt, value);
                }

                // Remove "-" characters while parsing Enum values.
                return Enum.Parse(_enumType, value.Replace("-", ""), true);
            }
            // TODO: Review code - exceptions are swallowed silently
            catch { }

            return value;
        }
    }
}