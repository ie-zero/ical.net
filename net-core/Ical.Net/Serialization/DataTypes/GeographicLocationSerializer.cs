using System;
using System.Globalization;
using System.IO;
using System.Text;
using Ical.Net.DataTypes;

namespace Ical.Net.Serialization.DataTypes
{
    public class GeographicLocationSerializer : EncodableDataTypeSerializer
    {
        public GeographicLocationSerializer(SerializationContext ctx) : base(ctx) { }

        public override Type TargetType => typeof(GeographicLocation);

        public override string SerializeToString(object obj)
        {
            var g = obj as GeographicLocation;
            if (g == null) { return null; }

            var value = new StringBuilder()
                .AppendFormat(CultureInfo.InvariantCulture.NumberFormat, "{0:0.000000}", g.Latitude)
                .Append(';')
                .AppendFormat(CultureInfo.InvariantCulture.NumberFormat, "{0:0.000000}", g.Longitude)
                .ToString();

            return Encode(g, value);
        }

        public GeographicLocation Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            var g = CreateAndAssociate() as GeographicLocation;
            if (g == null) { return null; }

            // Decode the value, if necessary!
            value = Decode(g, value);

            var values = value.Split(new [] {';'}, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length != 2)
            {
                return null;
            }

            // TODO: Parsing of GeographicLocation can succeed partially i.e. only Latitude or Longitude
            double.TryParse(values[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double lat);
            double.TryParse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double lon);
            g.Latitude = lat;
            g.Longitude = lon;

            return g;
        }

        public override object Deserialize(TextReader reader)
        {
            if (reader == null) return null;

            return Deserialize(reader.ReadToEnd());
        }
    }
}