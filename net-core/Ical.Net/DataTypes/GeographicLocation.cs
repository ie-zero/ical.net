using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ical.Net.Components;
using Ical.Net.Serialization;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// A class that represents the geographical location of an
    /// <see cref="ComponentName.Event"/> or <see cref="Todo"/> item.
    /// </summary>
    [DebuggerDisplay("{Latitude};{Longitude}")]
    public class GeographicLocation : EncodableDataType
    {
        public GeographicLocation() { }

        public GeographicLocation(string value) : this()
        {
            var serializer = new GeographicLocationSerializer(SerializationContext.Default);
            serializer.Deserialize(value);
        }

        public GeographicLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        // TODO: See if CopyFrom() method can be deleted.
        public override void CopyFrom(ICopyable obj) {}        
    }
}