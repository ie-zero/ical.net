using System;
using System.IO;
using Ical.Net.DataTypes;

namespace Ical.Net.Serialization.DataTypes
{
    public class OrganizerSerializer : StringSerializer
    {
        public OrganizerSerializer(SerializationContext ctx) : base(ctx) { }

        public override Type TargetType => typeof(Organizer);

        public override string SerializeToString(object obj)
        {
            try
            {
                var organizer = obj as Organizer;
                return organizer?.Value == null
                    ? null
                    : Encode(organizer, Escape(organizer.Value.OriginalString));
            }
            catch
            {
                return null;
            }
        }

        public override object Deserialize(TextReader reader)
        {
            var value = reader.ReadToEnd();

            try
            {
                var organizer = CreateAndAssociate() as Organizer;
                if (organizer == null) { return null; }

                var uriString = Unescape(Decode(organizer, value));

                // Prepend "mailto:" if necessary
                if (!uriString.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                {
                    uriString = $"mailto:{uriString}";
                }

                organizer.Value = new Uri(uriString);
                return organizer;
            }
            catch { }

            return null;
        }
    }
}