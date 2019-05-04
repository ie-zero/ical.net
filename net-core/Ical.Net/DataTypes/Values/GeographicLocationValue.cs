using System.Collections.Generic;

namespace Ical.Net.DataTypes.Values
{
    public sealed class GeographicLocationValue : ValueObject
    {
        public GeographicLocationValue() { }

        public GeographicLocationValue(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }

        public double Longitude { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Latitude;
            yield return Longitude;
        }
    }
}