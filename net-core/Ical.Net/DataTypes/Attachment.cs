using System;
using System.Linq;
using System.Text;
using Ical.Net.Serialization;
using Ical.Net.Serialization.DataTypes;
using Ical.Net.Utility;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// Attachments represent the ATTACH element that can be associated with Alarms, Journals, Todos, and Events. There are two kinds of attachments:
    /// 1) A string representing a URI which is typically human-readable, OR
    /// 2) A base64-encoded string that can represent anything
    /// </summary>
    public class Attachment : EncodableDataType
    {
        // TODO: Consider sub-classing Attachment class to represent the two distinct types.

        private Encoding _valueEncoding = System.Text.Encoding.UTF8;

        public Attachment() { }

        public Attachment(byte[] value) : this()
        {
            if (value != null)
            {
                Data = value;
            }
        }

        public Attachment(string value) : this()
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var serializer = new AttachmentSerializer(SerializationContext.Default);
            var a = serializer.Deserialize(value);
            if (a == null)
            {
                throw new ArgumentException($"The supplied value should be a valid ATTACH component", nameof(value));
            }

            ValueEncoding = a.ValueEncoding;

            Data = a.Data;
            Uri = a.Uri;
        }

        public byte[] Data { get; }

        public string FormatType
        {
            get => Parameters.Get("FMTTYPE");
            set => Parameters.Set("FMTTYPE", value);
        }

        public Uri Uri { get; set; }

        public Encoding ValueEncoding
        {
            get => _valueEncoding;
            set
            {
                if (value == null)
                {
                    return;
                }
                _valueEncoding = value;
            }
        }

        protected bool Equals(Attachment other)
        {
            var firstPart = Equals(Uri, other.Uri) && ValueEncoding.Equals(other.ValueEncoding);
            return Data == null
                ? firstPart
                : firstPart && Data.SequenceEqual(other.Data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Attachment)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Uri?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (CollectionHelpers.GetHashCode(Data));
                hashCode = (hashCode * 397) ^ (ValueEncoding?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        // TODO: Overriding ToString() method does not conform with the way the remaining library is implemented.
        public override string ToString()
        {
            return Data == null ? string.Empty : ValueEncoding.GetString(Data);
        }
    }
}