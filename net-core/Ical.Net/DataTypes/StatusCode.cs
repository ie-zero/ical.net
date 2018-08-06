using System.IO;
using System.Linq;
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
        public StatusCode() { }

        public StatusCode(int[] parts)
        {
            Parts = parts;
        }

        public StatusCode(string value) : this()
        {
            var serializer = new StatusCodeSerializer(SerializationContext.Default);
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        public int[] Parts { get; private set; }

        public int Primary
        {
            get { return Parts.Length > 0 ? Parts[0] : 0; }
        }

        public int Secondary
        {
            get { return Parts.Length > 1 ? Parts[1] : 0; }
        }

        public int Tertiary
        {
            get { return Parts.Length > 2 ? Parts[2] : 0; }
        }

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);
            if (copyable is StatusCode)
            {
                var sc = (StatusCode) copyable;
                Parts = new int[sc.Parts.Length];
                sc.Parts.CopyTo(Parts, 0);
            }
        }

        protected bool Equals(StatusCode other)
        {
            return Parts.SequenceEqual(other.Parts);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((StatusCode)obj);
        }

        public override int GetHashCode()
        {
            return CollectionHelpers.GetHashCode(Parts);
        }

        public override string ToString()
        {
            return new StatusCodeSerializer(SerializationContext.Default).SerializeToString(this);
        }
    }
}