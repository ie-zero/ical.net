using System.IO;
using Ical.Net.Serialization;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// Represents the return status of an iCalendar request.
    /// </summary>
    public class RequestStatus : EncodableDataType
    {
        public RequestStatus() { }

        public RequestStatus(string value)
        {
            var serializer = new RequestStatusSerializer(SerializationContext.Default);
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        public string Description { get; set; }

        public string ExtraData { get; set; }

        public StatusCode StatusCode { get; set; }

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            if (!(copyable is RequestStatus))
            {
                return;
            }

            var rs = (RequestStatus) copyable;
            if (rs.StatusCode != null)
            {
                StatusCode = rs.StatusCode;
            }
            Description = rs.Description;
            rs.ExtraData = rs.ExtraData;
        }

        protected bool Equals(RequestStatus other)
        {
            return string.Equals(Description, other.Description)
                && string.Equals(ExtraData, other.ExtraData)
                && Equals(StatusCode, other.StatusCode);
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
            return Equals((RequestStatus)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Description?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (ExtraData?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (StatusCode?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            var serializer = new RequestStatusSerializer(SerializationContext.Default);
            return serializer.SerializeToString(this);
        }
    }
}