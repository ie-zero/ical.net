using System;
using System.IO;
using Ical.Net.DataTypes;

namespace Ical.Net.Serialization.DataTypes
{
    public class OrganizerSerializer : StringSerializer
    {
        public OrganizerSerializer(SerializationContext ctx) : base(ctx) { }

        public override Type TargetType => typeof (Organizer);

        public override string SerializeToString(object obj)
        {
            var organizer = obj as Organizer;
            if (organizer?.Value == null) return null;

            try
            {
                // TODO: Review code as only the value property from Organiser is serialized.

                return Encode(organizer, Escape(organizer.Value.OriginalString));
            }
            catch
            {
                // TODO: Review code - exceptions are swallowed silently
                return null;
            }
        }

        public override object Deserialize(TextReader reader)
        {
            if (reader == null) return null;
            var value = reader.ReadToEnd();

            try
            {
                var organizer = CreateAndAssociate() as Organizer;
                if (organizer != null)
                {
                    var uriString = Unescape(Decode(organizer, value));

                    // Prepend "mailto:" if necessary
                    if (!uriString.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                    {
                        uriString = "mailto:" + uriString;
                    }

                    organizer.Value = new Uri(uriString);
                }

                return organizer;
            }
            catch
            {
                // TODO: Review code - exceptions are swallowed silently
                return null;
            }
        }
    }
}