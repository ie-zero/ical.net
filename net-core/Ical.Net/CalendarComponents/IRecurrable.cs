using System.Collections.Generic;
using Ical.Net.DataTypes;

namespace Ical.Net.CalendarComponents
{
    public interface IRecurrable : IGetOccurrences, ITypedServicesProvider
    {
        /// <summary>
        /// Gets/sets the start date/time of the component.
        /// </summary>
        CalDateTime Start { get; set; }

        IList<PeriodList> ExceptionDates { get; set; }
        IList<RecurrencePattern> ExceptionRules { get; set; }
        IList<PeriodList> RecurrenceDates { get; set; }
        IList<RecurrencePattern> RecurrenceRules { get; set; }
        CalDateTime RecurrenceId { get; set; }
    }
}