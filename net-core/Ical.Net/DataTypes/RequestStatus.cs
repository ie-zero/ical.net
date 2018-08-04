using System.IO;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// A class that represents the return status of an iCalendar request.
    /// </summary>
    public class RequestStatus : EncodableDataType
    {
        private string _description;
        private string _extraData;
        private StatusCode _statusCode;

        public RequestStatus() { }

        public RequestStatus(string value) : this()
        {
            var serializer = new RequestStatusSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public string ExtraData
        {
            get => _extraData;
            set => _extraData = value;
        }

        public StatusCode StatusCode
        {
            get => _statusCode;
            set => _statusCode = value;
        }

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
            return string.Equals(_description, other._description)
                && string.Equals(_extraData, other._extraData)
                && Equals(_statusCode, other._statusCode);
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
                var hashCode = _description?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (_extraData?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (_statusCode?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            var serializer = new RequestStatusSerializer();
            return serializer.SerializeToString(this);
        }
    }
}