using System;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.Utility;

namespace Ical.Net.DataTypes
{
    public class Attendee : EncodableDataType
    {
        private string _commonName;
        private List<string> _delegatedFrom;
        private List<string> _delegatedTo;
        private Uri _directoryEntry;
        private List<string> _members;
        private string _participationStatus;
        private string _role;
        private bool? _rsvp;
        private Uri _sentBy;
        private string _type;

        // TODO: Possibly obsolete. Consider removing.
        public Attendee() { }

        public Attendee(Uri attendee)
        {
            Value = attendee;
        }

        public Attendee(string attendeeUri)
        {
            if (!Uri.IsWellFormedUriString(attendeeUri, UriKind.Absolute))
            {
                // TODO: Improve thrown exception.
                throw new ArgumentException("attendeeUri");
            }
            Value = new Uri(attendeeUri);
        }

        /// <summary> CN: to show the common or displayable name associated with the calendar address </summary>
        public string CommonName
        {
            get
            {
                if (string.IsNullOrEmpty(_commonName))
                {
                    _commonName = Parameters.Get("CN");
                }
                return _commonName;
            }

            set
            {
                if (string.Equals(_commonName, value, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
                _commonName = value;
                Parameters.Set("CN", value);
            }
        }

        /// <summary> DELEGATED-FROM, to indicate whom the request was delegated from </summary>
        public IList<string> DelegatedFrom
        {
            get
            {
                return _delegatedFrom ?? (_delegatedFrom = new List<string>(Parameters.GetMany("DELEGATED-FROM")));
            }

            set
            {
                if (value == null)
                {
                    return;
                }
                _delegatedFrom = new List<string>(value);
                Parameters.Set("DELEGATED-FROM", value);
            }
        }

        /// <summary> DELEGATED-TO, to indicate the calendar users that the original request was delegated to </summary>
        public IList<string> DelegatedTo
        {
            get
            {
                return _delegatedTo ?? (_delegatedTo = new List<string>(Parameters.GetMany("DELEGATED-TO")));
            }

            set
            {
                if (value == null)
                {
                    return;
                }
                _delegatedTo = new List<string>(value);
                Parameters.Set("DELEGATED-TO", value);
            }
        }

        /// <summary> DIR, to indicate the URI that points to the directory information corresponding to the attendee </summary>
        public Uri DirectoryEntry
        {
            get
            {
                if (_directoryEntry != null)
                {
                    return _directoryEntry;
                }

                var newUrl = Parameters.Get("SENT-BY");
                Uri.TryCreate(newUrl, UriKind.RelativeOrAbsolute, out _directoryEntry);
                return _directoryEntry;
            }

            set
            {
                if (value == null || value == _directoryEntry)
                {
                    return;
                }
                _directoryEntry = value;
                Parameters.Set("DIR", value.OriginalString);
            }
        }

        /// <summary> MEMBER: the groups the user belongs to </summary>
        public IList<string> Members
        {
            get
            {
                return _members ?? (_members = new List<string>(Parameters.GetMany("MEMBER")));
            }

            set
            {
                _members = new List<string>(value);
                Parameters.Set("MEMBER", value);
            }
        }

        public string ParticipationStatus
        {
            get
            {
                if (string.IsNullOrEmpty(_participationStatus))
                {
                    _participationStatus = Parameters.Get(EventParticipationStatus.Key);
                }
                return _participationStatus;
            }

            set
            {
                if (string.Equals(_participationStatus, value, EventParticipationStatus.Comparison))
                {
                    return;
                }
                _participationStatus = value;
                Parameters.Set(EventParticipationStatus.Key, value);
            }
        }

        /// <summary> ROLE: the intended role the attendee will have </summary>
        public string Role
        {
            get
            {
                if (string.IsNullOrEmpty(_role))
                {
                    _role = Parameters.Get("ROLE");
                }
                return _role;
            }

            set
            {
                if (string.Equals(_role, value, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
                _role = value;
                Parameters.Set("ROLE", value);
            }
        }

        /// <summary> RSVP, to indicate whether a reply is requested </summary>
        public bool Rsvp
        {
            get
            {
                if (_rsvp != null)
                {
                    return _rsvp.Value;
                }

                bool val;
                var rsvp = Parameters.Get("RSVP");
                if (rsvp != null && bool.TryParse(rsvp, out val))
                {
                    _rsvp = val;
                    return _rsvp.Value;
                }
                return false;
            }

            set
            {
                _rsvp = value;
                var val = value.ToString().ToUpperInvariant();
                Parameters.Set("RSVP", val);
            }
        }

        /// <summary> SENT-BY, to indicate who is acting on behalf of the ATTENDEE </summary>
        public Uri SentBy
        {
            get
            {
                if (_sentBy != null)
                {
                    return _sentBy;
                }

                var newUrl = Parameters.Get("SENT-BY");
                Uri.TryCreate(newUrl, UriKind.RelativeOrAbsolute, out _sentBy);
                return _sentBy;
            }

            set
            {
                if (value == null || value == _sentBy)
                {
                    return;
                }
                _sentBy = value;
                Parameters.Set("SENT-BY", value.OriginalString);
            }
        }

        /// <summary> CUTYPE: the type of calendar user </summary>
        public string Type
        {
            get
            {
                if (string.IsNullOrEmpty(_type))
                {
                    _type = Parameters.Get("CUTYPE");
                }
                return _type;
            }

            set
            {
                if (string.Equals(_type, value, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                _type = value;
                Parameters.Set("CUTYPE", value);
            }
        }

        /// <summary> Uri associated with the attendee, typically an email address </summary>
        public Uri Value { get; set; }

        // TODO: See if CopyFrom() method can be deleted.
        public override void CopyFrom(ICopyable obj) { }

        protected bool Equals(Attendee other)
        {
            return Equals(SentBy, other.SentBy)
                && string.Equals(CommonName, other.CommonName, StringComparison.OrdinalIgnoreCase)
                && Equals(DirectoryEntry, other.DirectoryEntry)
                && string.Equals(Type, other.Type, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Role, other.Role)
                && string.Equals(ParticipationStatus, other.ParticipationStatus, StringComparison.OrdinalIgnoreCase)
                && Rsvp == other.Rsvp
                && Equals(Value, other.Value)
                && Members.SequenceEqual(other.Members)
                && DelegatedTo.SequenceEqual(other.DelegatedTo)
                && DelegatedFrom.SequenceEqual(other.DelegatedFrom);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Attendee)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = SentBy?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (CommonName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (DirectoryEntry?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Type?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(Members);
                hashCode = (hashCode * 397) ^ (Role?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (ParticipationStatus?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Rsvp.GetHashCode();
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(DelegatedTo);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(DelegatedFrom);
                hashCode = (hashCode * 397) ^ (Value?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}