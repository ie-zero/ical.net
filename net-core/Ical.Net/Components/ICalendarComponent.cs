namespace Ical.Net.Components
{
    public interface ICalendarComponent : ICalendarObject
    {
        CalendarPropertyList Properties { get; }
    }
}