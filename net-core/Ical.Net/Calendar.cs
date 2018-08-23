using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Ical.Net.Components;
using Ical.Net.DataTypes;
using Ical.Net.Extensions;
using Ical.Net.Proxies;
using Ical.Net.Serialization;
using Ical.Net.Utilities;

namespace Ical.Net
{
    public class Calendar : CalendarComponent, IGetOccurrences, IGetOccurrencesTyped
    {
        /// <summary>
        /// To load an existing an iCalendar object, use one of the provided LoadFromXXX methods.
        /// <example>
        /// For example, use the following code to load an iCalendar object from a URL:
        /// <code>
        ///     IICalendar iCal = iCalendar.LoadFromUri(new Uri("http://somesite.com/calendar.ics"));
        /// </code>
        /// </example>
        /// </summary>
        public Calendar() : base(ComponentName.Calendar)
        {
            Initialize();
        }

        private void Initialize()
        {
            UniqueComponents = new UniqueComponentListProxy<IUniqueComponent>(Children);
            Events = new UniqueComponentListProxy<CalendarEvent>(Children);
            Todos = new UniqueComponentListProxy<Todo>(Children);
            Journals = new CalendarObjectListProxy<Journal>(Children);
            FreeBusy = new UniqueComponentListProxy<FreeBusy>(Children);
            TimeZones = new CalendarObjectListProxy<VTimeZone>(Children);
        }
        
        public string Method
        {
            get => Properties.Get<string>("METHOD");
            set => Properties.Set("METHOD", value);
        }

        public string ProductId
        {
            get => Properties.Get<string>("PRODID");
            set => Properties.Set("PRODID", value);
        }

        public RecurrenceEvaluationModeType RecurrenceEvaluationMode
        {
            get => Properties.Get<RecurrenceEvaluationModeType>("X-DDAY-ICAL-RECURRENCE-EVALUATION-MODE");
            set => Properties.Set("X-DDAY-ICAL-RECURRENCE-EVALUATION-MODE", value);
        }

        public RecurrenceRestrictionType RecurrenceRestriction
        {
            get => Properties.Get<RecurrenceRestrictionType>("X-DDAY-ICAL-RECURRENCE-RESTRICTION");
            set => Properties.Set("X-DDAY-ICAL-RECURRENCE-RESTRICTION", value);
        }

        public string Version
        {
            get => Properties.Get<string>("VERSION");
            set => Properties.Set("VERSION", value);
        }

        public string Scale
        {
            get => Properties.Get<string>("CALSCALE");
            set => Properties.Set("CALSCALE", value);
        }

        public IUniqueComponentList<IUniqueComponent> UniqueComponents { get; private set; }

        /// <summary>
        /// A collection of <see cref="ComponentName.Event"/> components in the iCalendar.
        /// </summary>
        public IUniqueComponentList<CalendarEvent> Events { get; private set; }

        /// <summary>
        /// A collection of <see cref="Todo"/> components in the iCalendar.
        /// </summary>
        public IUniqueComponentList<Todo> Todos { get; private set; }

        /// <summary>
        /// A collection of <see cref="Journal"/> components in the iCalendar.
        /// </summary>
        public ICalendarObjectList<Journal> Journals { get; private set; }

        /// <summary>
        /// A collection of <see cref="Components.FreeBusy"/> components in the iCalendar.
        /// </summary>
        public IUniqueComponentList<FreeBusy> FreeBusy { get; private set; }

        /// <summary>
        /// A collection of VTimeZone components in the iCalendar.
        /// </summary>
        public ICalendarObjectList<VTimeZone> TimeZones { get; private set; }

        public IEnumerable<IRecurrable> RecurringItems
        {
            get { return Children.OfType<IRecurrable>(); }
        }

        public static Calendar Load(string calendarString)
        {
            using (var reader = new StringReader(calendarString))
            {
                return Load(reader);
            }
        }

        /// <summary>
        /// Loads an <see cref="Calendar"/> from an open stream.
        /// </summary>
        /// <param name="stream">The stream from which to load the <see cref="Calendar"/> object</param>
        /// <returns>An <see cref="Calendar"/> object</returns>
        public static Calendar Load(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return Load(reader);
            }
        }

        public static Calendar Load(TextReader reader)
        {
            return SimpleDeserializer.Default.Deserialize(reader).OfType<Calendar>().SingleOrDefault();
        }

        public static IEnumerable<T> LoadMany<T>(Stream stream, Encoding encoding)
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                return LoadMany<T>(reader);
            }
        }

        public static IEnumerable<T> LoadMany<T>(TextReader reader)
        {
            return SimpleDeserializer.Default.Deserialize(reader).OfType<T>().ToArray();
        }

        public static IEnumerable<T> LoadMany<T>(string calendarString)
        {
            using (var reader = new StringReader(calendarString))
            {
                return LoadMany<T>(reader);
            }
        }

        public static IEnumerable<Calendar> LoadMany(Stream stream, Encoding encoding)
        {
            return LoadMany<Calendar>(stream, encoding);
        }

        public static IEnumerable<Calendar> LoadMany(TextReader reader)
        {
            return LoadMany<Calendar>(reader);
        }

        public static IEnumerable<Calendar> LoadMany(string calendarString)
        {
            return LoadMany<Calendar>(calendarString);
        }

        public VTimeZone AddLocalTimeZone(DateTime earliestDateTimeToSupport, bool includeHistoricalData)
        {
            var tz = VTimeZone.FromLocalTimeZone(earliestDateTimeToSupport, includeHistoricalData);
            this.AddChild(tz);
            return tz;
        }

        /// <summary>
        /// Adds a time zone to the iCalendar. This time zone may then be used in date/time objects
        /// contained in the calendar.
        /// </summary>
        /// <returns>The time zone added to the calendar.</returns>
        public VTimeZone AddTimeZone(VTimeZone tz)
        {
            this.AddChild(tz);
            return tz;
        }

        /// <summary>
        /// Adds a system time zone to the iCalendar. This time zone may then be used in date/time
        /// objects contained in the calendar.
        /// </summary>
        /// <param name="timeZoneInfo">A System.TimeZoneInfo object to add to the calendar.</param>
        /// <returns>The time zone added to the calendar.</returns>
        public VTimeZone AddTimeZone(TimeZoneInfo timeZoneInfo)
        {
            var tz = VTimeZone.FromSystemTimeZone(timeZoneInfo);
            this.AddChild(tz);
            return tz;
        }

        public VTimeZone AddTimeZone(TimeZoneInfo timeZoneInfo, DateTime earliestDateTimeToSupport, bool includeHistoricalData)
        {
            var tz = VTimeZone.FromSystemTimeZone(timeZoneInfo, earliestDateTimeToSupport, includeHistoricalData);
            this.AddChild(tz);
            return tz;
        }

        public VTimeZone AddTimeZone(string tzId)
        {
            var tz = VTimeZone.FromDateTimeZone(tzId);
            this.AddChild(tz);
            return tz;
        }

        public VTimeZone AddTimeZone(string tzId, DateTime earliestDateTimeToSupport, bool includeHistoricalData)
        {
            var tz = VTimeZone.FromDateTimeZone(tzId, earliestDateTimeToSupport, includeHistoricalData);
            this.AddChild(tz);
            return tz;
        }

        /// <summary>
        /// Clears recurrence evaluations for recurring components.        
        /// </summary>        
        public void ClearEvaluation()
        {
            foreach (var recurrable in RecurringItems)
            {
                recurrable.ClearEvaluation();
            }
        }

        /// <summary>
        /// Creates a typed object that is a direct child of the iCalendar itself. Generally, you
        /// would invoke this method to create an Event, Todo, Journal, VTimeZone, FreeBusy, or other
        /// base component type.
        /// </summary>
        /// <example>
        /// To create an event, use the following:
        /// <code>
        /// IICalendar iCal = new iCalendar();
        ///
        /// Event evt = iCal.Create&lt;Event&gt;();
        /// </code>
        /// This creates the event, and adds it to the Events list of the iCalendar.
        /// </example>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <returns>An object of the type specified</returns>
        public T Create<T>() where T : ICalendarComponent
        {
            var obj = Activator.CreateInstance(typeof(T)) as ICalendarObject;
            if (obj is T)
            {
                this.AddChild(obj);
                return (T)obj;
            }
            return default(T);
        }

        public Calendar Copy()
        {
            var calendar = new Calendar();
            calendar.CopyFrom(this);

            return calendar;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj.GetType() == GetType() && Equals((Calendar)obj);
        }

        protected bool Equals(Calendar other)
        {
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase)
                           && CollectionHelpers.Equals(UniqueComponents, other.UniqueComponents)
                           && CollectionHelpers.Equals(Events, other.Events)
                           && CollectionHelpers.Equals(Todos, other.Todos)
                           && CollectionHelpers.Equals(Journals, other.Journals)
                           && CollectionHelpers.Equals(FreeBusy, other.FreeBusy)
                           && CollectionHelpers.Equals(TimeZones, other.TimeZones);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(UniqueComponents);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(Events);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(Todos);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(Journals);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(FreeBusy);
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(TimeZones);
                return hashCode;
            }
        }

        public FreeBusy GetFreeBusy(FreeBusy freeBusyRequest)
        {
            return Components.FreeBusy.Create(this, freeBusyRequest);
        }

        public FreeBusy GetFreeBusy(IDateTime fromInclusive, IDateTime toExclusive)
        {
            return Components.FreeBusy.Create(this, Components.FreeBusy.CreateRequest(fromInclusive, toExclusive, null, null));
        }

        public FreeBusy GetFreeBusy(Organizer organizer, IEnumerable<Attendee> contacts, IDateTime fromInclusive, IDateTime toExclusive)
        {
            return Components.FreeBusy.Create(this, Components.FreeBusy.CreateRequest(fromInclusive, toExclusive, organizer, contacts));
        }

        /// <summary>
        /// Returns a list of occurrences of each recurring component
        /// for the date provided (<paramref name="dt"/>).
        /// </summary>
        /// <param name="dt">The date for which to return occurrences. Time is ignored on this parameter.</param>
        /// <returns>A list of occurrences that occur on the given date (<paramref name="dt"/>).</returns>
        public HashSet<Occurrence> GetOccurrences(IDateTime dt)
        {
            return GetOccurrences<IRecurringComponent>(new CalDateTime(dt.GetAsSystemLocal().Date), new CalDateTime(dt.GetAsSystemLocal().Date.AddDays(1).AddSeconds(-1)));
        }

        public HashSet<Occurrence> GetOccurrences(DateTime dt)
        {
            return GetOccurrences<IRecurringComponent>(new CalDateTime(dt.Date), new CalDateTime(dt.Date.AddDays(1).AddSeconds(-1)));
        }

        /// <summary>
        /// Returns a list of occurrences of each recurring component
        /// that occur between <paramref name="startTime"/> and <paramref name="endTime"/>.
        /// </summary>
        /// <param name="startTime">The beginning date/time of the range.</param>
        /// <param name="endTime">The end date/time of the range.</param>
        /// <returns>A list of occurrences that fall between the dates provided.</returns>
        public HashSet<Occurrence> GetOccurrences(IDateTime startTime, IDateTime endTime)
        {
            return GetOccurrences<IRecurringComponent>(startTime, endTime);
        }

        public HashSet<Occurrence> GetOccurrences(DateTime startTime, DateTime endTime)
        {
            return GetOccurrences<IRecurringComponent>(new CalDateTime(startTime), new CalDateTime(endTime));
        }

        /// <summary>
        /// Returns all occurrences of components of type T that start on the date provided.
        /// All components starting between 12:00:00AM and 11:59:59 PM will be
        /// returned.
        /// <note>
        /// This will first Evaluate() the date range required in order to
        /// determine the occurrences for the date provided, and then return
        /// the occurrences.
        /// </note>
        /// </summary>
        /// <param name="dt">The date for which to return occurrences.</param>
        /// <returns>A list of Periods representing the occurrences of this object.</returns>
        public HashSet<Occurrence> GetOccurrences<T>(IDateTime dt) where T : IRecurringComponent
        {
            return GetOccurrences<T>(new CalDateTime(dt.GetAsSystemLocal().Date), new CalDateTime(dt.GetAsSystemLocal().Date.AddDays(1).AddTicks(-1)));
        }

        public HashSet<Occurrence> GetOccurrences<T>(DateTime dt) where T : IRecurringComponent
        {
            return GetOccurrences<T>(new CalDateTime(dt.Date), new CalDateTime(dt.Date.AddDays(1).AddTicks(-1)));
        }

        /// <summary>
        /// Returns all occurrences of components of type T that start within the date range provided.
        /// 
        /// All components occurring between <paramref name="startTime"/> and <paramref name="endTime"/>
        /// will be returned.
        /// </summary>
        /// <typeparam name="T">Occurances will be returned only for recurring components of this type.</typeparam>
        /// <param name="startTime">The starting date range</param>
        /// <param name="endTime">The ending date range</param>
        public HashSet<Occurrence> GetOccurrences<T>(IDateTime startTime, IDateTime endTime) where T : IRecurringComponent
        {
            var occurrences = new HashSet<Occurrence>(RecurringItems
                .OfType<T>()
                .SelectMany(recurrable => recurrable.GetOccurrences(startTime, endTime)));

            var removeOccurrencesQuery = occurrences
                .Where(o => o.Source is UniqueComponent)
                .GroupBy(o => ((UniqueComponent)o.Source).Uid)
                .SelectMany(group => group
                    .Where(o => o.Source.RecurrenceId != null)
                    .SelectMany(occurrence => group.
                        Where(o => o.Source.RecurrenceId == null && occurrence.Source.RecurrenceId.Date.Equals(o.Period.StartTime.Date))));

            occurrences.ExceptWith(removeOccurrencesQuery);
            return occurrences;
        }

        public HashSet<Occurrence> GetOccurrences<T>(DateTime startTime, DateTime endTime) where T : IRecurringComponent
        {
            return GetOccurrences<T>(new CalDateTime(startTime), new CalDateTime(endTime));
        }

        /// <summary>
        /// Merges the properties and components of another calendar into this one, when they do not exist in the target object.
        /// </summary>
        public void MergeWith(Calendar calendar)
        {
            if (calendar == null) { return; }

            if (Name == null)
            {
                Name = calendar.Name;
            }

            Method = calendar.Method;
            Version = calendar.Version;
            ProductId = calendar.ProductId;
            Scale = calendar.Scale;

            foreach (var property in calendar.Properties.Where(p => !Properties.ContainsKey(p.Name)))
            {
                Properties.Add(property);
            }

            foreach (var child in calendar.Children)
            {
                if (child is IUniqueComponent)
                {
                    if (!UniqueComponents.ContainsKey(((IUniqueComponent)child).Uid))
                    {
                        this.AddChild(child);
                    }
                }
                else
                {
                    this.AddChild(child);
                }
            }
        }

        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);

            Initialize();
        }
    }
}