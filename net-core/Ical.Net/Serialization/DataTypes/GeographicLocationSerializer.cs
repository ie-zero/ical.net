using System;
using System.Globalization;
using System.IO;
using Ical.Net.DataTypes;

namespace Ical.Net.Serialization.DataTypes
{
    public class GeographicLocationSerializer : EncodableDataTypeSerializer
    {
        public GeographicLocationSerializer(SerializationContext ctx) : base(ctx) { }

        public override Type TargetType => typeof(GeographicLocation);

        public override string SerializeToString(object obj)
        {
            var geoLocation = obj as GeographicLocation;
            if (geoLocation == null) { return null; }

            var value = geoLocation.Latitude.ToString("0.000000", CultureInfo.InvariantCulture.NumberFormat) + ";"
                      + geoLocation.Longitude.ToString("0.000000", CultureInfo.InvariantCulture.NumberFormat);
            return Encode(geoLocation, value);
        }

        public GeographicLocation Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return null; }

            var geoLocation = CreateAndAssociate() as GeographicLocation;
            if (geoLocation == null) { return null; }

            // Decode the value, if necessary!
            value = Decode(geoLocation, value);

            var values = value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length != 2) { return null; }

            double.TryParse(values[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude);
            double.TryParse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude);

            geoLocation.Latitude = latitude;
            geoLocation.Longitude = longitude;
            return geoLocation;
        }

        public override object Deserialize(TextReader reader)
        {
            return Deserialize(reader.ReadToEnd());
        }
    }
}