using System.Collections.Generic;
using Ical.Net.DataTypes.Values;

namespace Ical.Net.DataTypes.Values
{
    public class StatusCodeValue : ValueObject
    {
        public StatusCodeValue(int[] parts)
        {
            Parts = parts;
        }

        public int[] Parts { get; }

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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Parts;
        }
    }
}