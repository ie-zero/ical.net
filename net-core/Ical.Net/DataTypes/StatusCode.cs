using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ical.Net.DataTypes.Values;
using Ical.Net.Serialization;
using Ical.Net.Serialization.DataTypes;
using Ical.Net.Utility;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// An iCalendar status code.
    /// </summary>
    public class StatusCode : EncodableDataType
    {
        private StatusCodeValue _value;

        public int[] Parts => _value.Parts;

        public int Primary => _value.Primary;

        public int Secondary => _value.Secondary;

        public int Tertiary => _value.Tertiary;

        public StatusCode()
        {
            _value = new StatusCodeValue(Array.Empty<int>());
        }

        public StatusCode(int[] parts)
        {
            _value = new StatusCodeValue(parts);
        }

        public StatusCode(string value) : this()
        {
            var serializer = new StatusCodeSerializer(SerializationContext.Default);
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        public override void CopyFrom(ICopyable obj)
        {
            base.CopyFrom(obj);

            var sc = obj as StatusCode;
            if (sc == null) return;

            _value = new StatusCodeValue(sc.Parts);
        }

        public override string ToString()
        {
            return new StatusCodeSerializer(SerializationContext.Default).SerializeToString(this);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            var other = obj as StatusCode;
            if (other == null) return false;

            return _value.Equals(other._value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}