using System;
using System.Diagnostics;
using System.IO;
using Ical.Net.Serialization;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// A class that represents the organizer of an event/todo/journal.
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public class Organizer : EncodableDataType
    {
        public Organizer() { }

        public Organizer(string value) : this()
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new OrganizerSerializer(SerializationContext.Default);
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        public string CommonName
        {
            get => Parameters.Get("CN");
            set => Parameters.Set("CN", value);
        }

        public Uri DirectoryEntry
        {
            get => new Uri(Parameters.Get("DIR"));
            set
            {
                if (value != null)
                {
                    Parameters.Set("DIR", value.OriginalString);
                }
                else
                {
                    Parameters.Set("DIR", (string)null);
                }
            }
        }

        public Uri SentBy
        {
            get => new Uri(Parameters.Get("SENT-BY"));
            set
            {
                if (value != null)
                {
                    Parameters.Set("SENT-BY", value.OriginalString);
                }
                else
                {
                    Parameters.Set("SENT-BY", (string) null);
                }
            }
        }

        public Uri Value { get; set; }

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            var organizer = copyable as Organizer;
            if (organizer == null) { return; }

            CopyFrom(organizer);
        }

        private void CopyFrom(Organizer organizer)
        {
            Value = organizer.Value;
        }

        protected bool Equals(Organizer other)
        {
            return Equals(Value, other.Value);
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
            return Equals((Organizer)obj);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}