using System;
using System.IO;
using Ical.Net.DataTypes;

namespace Ical.Net.Serialization.DataTypes
{
    public class EnumSerializer : EncodableDataTypeSerializer
    {
        private readonly Type _enumType;

        public EnumSerializer(Type enumType, SerializationContext ctx) : base(ctx)
        {
            _enumType = enumType;
        }

        public override Type TargetType => _enumType;

        public override string SerializeToString(object enumValue)
        {
            try
            {
                var obj = SerializationContext.Peek() as ICalendarObject;
                if (obj != null)
                {
                    // Encode the value as needed.
                    var dataType = new EncodableDataType
                    {
                        AssociatedObject = obj
                    };
                    return Encode(dataType, enumValue.ToString());
                }
                return enumValue.ToString();
            }
            catch
            {
                return null;
            }
        }

        public override object Deserialize(TextReader reader)
        {
            var value = reader.ReadToEnd();

            try
            {
                var obj = SerializationContext.Peek() as ICalendarObject;
                if (obj != null)
                {
                    // Decode the value, if necessary!
                    var dataType = new EncodableDataType
                    {
                        AssociatedObject = obj
                    };
                    value = Decode(dataType, value);
                }

                // Remove "-" characters while parsing Enum values.
                return Enum.Parse(_enumType, value.Replace("-", ""), true);
            }
            catch 
            {
                // TODO: We should NOT swallow the all the exceptions.
            }

            return value;
        }
    }
}