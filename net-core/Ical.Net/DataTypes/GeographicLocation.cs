using System.Collections.Generic;
using System.Diagnostics;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes.Values;
using Ical.Net.Serialization;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.DataTypes
{

    /// <summary>
    /// A class that represents the geographical location of an
    /// <see cref="Components.Event"/> or <see cref="Todo"/> item.
    /// </summary>
    [DebuggerDisplay("{Latitude};{Longitude}")]
    public class GeographicLocation : EncodableDataType
    {
        private GeographicLocationValue _value;

        public double Latitude
        {
            get => _value.Latitude;
            set => _value = new GeographicLocationValue(value, _value.Longitude);
        }

        public double Longitude
        {
            get => _value.Longitude;
            set => _value = new GeographicLocationValue(_value.Latitude, value);
        }

        public GeographicLocation()
        {
            _value = new GeographicLocationValue();
        }

        public GeographicLocation(string value) : this()
        {
            var serializer = new GeographicLocationSerializer(SerializationContext.Default);
            serializer.Deserialize(value);
        }

        public GeographicLocation(double latitude, double longitude)
        {
            _value = new GeographicLocationValue(latitude, longitude);
        }

        public override void CopyFrom(ICopyable obj)
        {
            var g = obj as GeographicLocation;
            if (obj == null) return;

            _value = new GeographicLocationValue(g.Latitude, g.Longitude);
        }

        public override string ToString() => Latitude.ToString("0.000000") + ";" + Longitude.ToString("0.000000");

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as GeographicLocation;
            if (other == null) return false;
            return _value.Equals(other._value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Latitude.GetHashCode() * 397) ^ Longitude.GetHashCode();
            }
        }
    }
}