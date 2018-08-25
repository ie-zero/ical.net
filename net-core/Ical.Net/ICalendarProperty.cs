using Ical.Net.Collections.Interfaces;
using Ical.Net.DataTypes;

namespace Ical.Net
{
    public interface ICalendarProperty : IPropertyParameters, ICalendarObject, IValueObject<object>
    {
        object Value { get; }
    }
}