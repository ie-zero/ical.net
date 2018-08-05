namespace Ical.Net.CalendarComponents
{
    public interface ICalendarComponent : ICalendarObject
    {
        CalendarPropertyList Properties { get; }
    }
}