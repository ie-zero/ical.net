using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Ical.Net.DataTypes;
using Ical.Net.Evaluation;
using Ical.Net.Proxies;
using Ical.Net.Utility;

namespace Ical.Net.CalendarComponents
{
    /// <summary>
    /// An iCalendar component that recurs.
    /// </summary>
    /// <remarks>
    /// This component automatically handles
    /// RRULEs, RDATE, EXRULEs, and EXDATEs, as well as the DTSTART
    /// for the recurring item (all recurring items must have a DTSTART).
    /// </remarks>
    public class RecurringComponent : UniqueComponent, IRecurringComponent
    {
        public static IEnumerable<IRecurringComponent> SortByDate(IEnumerable<IRecurringComponent> list) => SortByDate<IRecurringComponent>(list);

        public static IEnumerable<TRecurringComponent> SortByDate<TRecurringComponent>(IEnumerable<TRecurringComponent> list) => list.OrderBy(d => d);

        protected virtual bool EvaluationIncludesReferenceDate => false;

        public IList<Attachment> Attachments
        {
            get => Properties.GetMany<Attachment>("ATTACH");
            set => Properties.Set("ATTACH", value);
        }

        public IList<string> Categories
        {
            get => Properties.GetMany<string>("CATEGORIES");
            set => Properties.Set("CATEGORIES", value);
        }

        public string Class
        {
            get => Properties.Get<string>("CLASS");
            set => Properties.Set("CLASS", value);
        }

        public IList<string> Contacts
        {
            get => Properties.GetMany<string>("CONTACT");
            set => Properties.Set("CONTACT", value);
        }

        public CalDateTime Created
        {
            get => Properties.Get<CalDateTime>("CREATED");
            set => Properties.Set("CREATED", value);
        }

        public string Description
        {
            get => Properties.Get<string>("DESCRIPTION");
            set => Properties.Set("DESCRIPTION", value);
        }

        /// <summary>
        /// The start date/time of the component.
        /// </summary>
        public virtual CalDateTime DtStart
        {
            get => Properties.Get<CalDateTime>("DTSTART");
            set => Properties.Set("DTSTART", value);
        }

        public IList<PeriodList> ExceptionDates
        {
            get => Properties.GetMany<PeriodList>("EXDATE");
            set => Properties.Set("EXDATE", value);
        }

        public IList<RecurrencePattern> ExceptionRules
        {
            get => Properties.GetMany<RecurrencePattern>("EXRULE");
            set => Properties.Set("EXRULE", value);
        }

        public CalDateTime LastModified
        {
            get => Properties.Get<CalDateTime>("LAST-MODIFIED");
            set => Properties.Set("LAST-MODIFIED", value);
        }

        public int Priority
        {
            get => Properties.Get<int>("PRIORITY");
            set => Properties.Set("PRIORITY", value);
        }

        public IList<PeriodList> RecurrenceDates
        {
            get => Properties.GetMany<PeriodList>("RDATE");
            set => Properties.Set("RDATE", value);
        }

        public IList<RecurrencePattern> RecurrenceRules
        {
            get => Properties.GetMany<RecurrencePattern>("RRULE");
            set => Properties.Set("RRULE", value);
        }

        public CalDateTime RecurrenceId
        {
            get => Properties.Get<CalDateTime>("RECURRENCE-ID");
            set => Properties.Set("RECURRENCE-ID", value);
        }

        public IList<string> RelatedComponents
        {
            get => Properties.GetMany<string>("RELATED-TO");
            set => Properties.Set("RELATED-TO", value);
        }

        public int Sequence
        {
            get => Properties.Get<int>("SEQUENCE");
            set => Properties.Set("SEQUENCE", value);
        }

        /// <summary>
        /// An alias to the DTStart field (i.e. start date/time).
        /// </summary>
        public CalDateTime Start
        {
            get => DtStart;
            set => DtStart = value;
        }

        public string Summary
        {
            get => Properties.Get<string>("SUMMARY");
            set => Properties.Set("SUMMARY", value);
        }

        /// <summary>
        /// A list of <see cref="Alarm"/>s for this recurring component.
        /// </summary>
        public ICalendarObjectList<Alarm> Alarms => new CalendarObjectListProxy<Alarm>(Children);

        public RecurringComponent(string name) : base(name)
        {
            Initialize();
            EnsureProperties();
        }

        private void Initialize() => SetService(new RecurringEvaluator(this));

        private void EnsureProperties()
        {
            if (!Properties.ContainsKey("SEQUENCE"))
            {
                Sequence = 0;
            }
        }

        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);

            Initialize();
        }

        public void ClearEvaluation() => RecurrenceUtil.ClearEvaluation(this);

        public HashSet<Occurrence> GetOccurrences(CalDateTime dt) => RecurrenceUtil.GetOccurrences(this, dt, EvaluationIncludesReferenceDate);

        public HashSet<Occurrence> GetOccurrences(DateTime dt)
            => RecurrenceUtil.GetOccurrences(this, new CalDateTime(dt), EvaluationIncludesReferenceDate);

        public HashSet<Occurrence> GetOccurrences(CalDateTime startTime, CalDateTime endTime)
            => RecurrenceUtil.GetOccurrences(this, startTime, endTime, EvaluationIncludesReferenceDate);

        public HashSet<Occurrence> GetOccurrences(DateTime startTime, DateTime endTime)
            => RecurrenceUtil.GetOccurrences(this, new CalDateTime(startTime), new CalDateTime(endTime), EvaluationIncludesReferenceDate);

        public IList<AlarmOccurrence> PollAlarms() => PollAlarms(null, null);

        public IList<AlarmOccurrence> PollAlarms(CalDateTime startTime, CalDateTime endTime)
            => Alarms?.SelectMany(a => a.Poll(startTime, endTime)).ToList()
                ?? new List<AlarmOccurrence>();

        protected bool Equals(RecurringComponent other)
        {
            var result = Equals(DtStart, other.DtStart)
                && Equals(Priority, other.Priority)
                && string.Equals(Summary, other.Summary, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Class, other.Class, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Description, other.Description, StringComparison.OrdinalIgnoreCase)
                && Equals(RecurrenceId, other.RecurrenceId)
                && Attachments.SequenceEqual(other.Attachments)
                && CollectionHelpers.Equals(Categories, other.Categories)
                && CollectionHelpers.Equals(Contacts, other.Contacts)
                && CollectionHelpers.Equals(ExceptionDates, other.ExceptionDates)
                && CollectionHelpers.Equals(ExceptionRules, other.ExceptionRules)
                && CollectionHelpers.Equals(RecurrenceDates, other.RecurrenceDates, orderSignificant: true)
                && CollectionHelpers.Equals(RecurrenceRules, other.RecurrenceRules, orderSignificant: true);

            return result;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RecurringComponent)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DtStart?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ Priority.GetHashCode();
                hashCode = (hashCode * 397) ^ (Summary?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Class?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Description?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (RecurrenceId?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(Attachments);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(Categories);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(Contacts);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(ExceptionDates);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(ExceptionRules);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(RecurrenceDates);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(RecurrenceRules);
                return hashCode;
            }
        }
    }
}