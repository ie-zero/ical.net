using System;
using System.IO;
using System.Text.RegularExpressions;
using Ical.Net.DataTypes;

namespace Ical.Net.Serialization.DataTypes
{
    public class WeekDaySerializer : EncodableDataTypeSerializer
    {
        public WeekDaySerializer() { }

        public WeekDaySerializer(SerializationContext ctx) : base(ctx) { }

        public override Type TargetType => typeof(WeekDay);

        public override string SerializeToString(object obj)
        {
            var weekDay = obj as WeekDay;
            if (weekDay == null) return null; 

            var value = string.Empty;
            if (weekDay.Offset != int.MinValue)
            {
                value += weekDay.Offset;
            }
            value += Enum.GetName(typeof (DayOfWeek), weekDay.DayOfWeek).ToUpper().Substring(0, 2);

            return Encode(weekDay, value);
        }

        private static readonly Regex _dayOfWeek = new Regex(@"(\+|-)?(\d{1,2})?(\w{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override object Deserialize(TextReader reader)
        {
            if (reader == null) return null;

            var value = reader.ReadToEnd();

            // Create the day specifier and associate it with a calendar object
            var weekDay = CreateAndAssociate() as WeekDay;

            // Decode the value, if necessary
            value = Decode(weekDay, value);

            var match = _dayOfWeek.Match(value);
            if (!match.Success)
            {
                return null;
            }

            if (match.Groups[2].Success)
            {
                weekDay.Offset = Convert.ToInt32(match.Groups[2].Value);
                if (match.Groups[1].Success && match.Groups[1].Value.Contains("-"))
                {
                    weekDay.Offset *= -1;
                }
            }
            weekDay.DayOfWeek = RecurrencePatternSerializer.GetDayOfWeek(match.Groups[3].Value);
            return weekDay;
        }
    }
}