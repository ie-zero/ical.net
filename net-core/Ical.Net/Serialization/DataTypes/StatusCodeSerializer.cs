using System;
using System.IO;
using System.Text.RegularExpressions;
using Ical.Net.DataTypes;

namespace Ical.Net.Serialization.DataTypes
{
    public class StatusCodeSerializer : StringSerializer
    {
        private static readonly Regex StatusCodeRegEx = new Regex(@"\d(\.\d+)*", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public StatusCodeSerializer(SerializationContext ctx) : base(ctx) { }

        public override Type TargetType => typeof(StatusCode);

        public override string SerializeToString(object obj)
        {
            var statusCode = obj as StatusCode;
            if (statusCode == null) { return null; }

            var values = new string[statusCode.Parts.Length];
            for (var i = 0; i < statusCode.Parts.Length; i++)
            {
                values[i] = statusCode.Parts[i].ToString();
            }
            return Encode(statusCode, Escape(string.Join(".", values)));
        }

        public override object Deserialize(TextReader reader)
        {
            var value = reader.ReadToEnd();

            var statusCode = CreateAndAssociate() as StatusCode;
            if (statusCode == null) { return null; }

            // Decode the value as needed
            value = Decode(statusCode, value);

            var match = StatusCodeRegEx.Match(value);
            if (!match.Success) { return null; }

            var parts = match.Value.Split('.');
            var numericParts = new int[parts.Length];
            for (var i = 0; i < parts.Length; i++)
            {
                if (!int.TryParse(parts[i], out int num))
                {
                    return false;
                }
                numericParts[i] = num;
            }

            return new StatusCode(numericParts);
        }
    }
}