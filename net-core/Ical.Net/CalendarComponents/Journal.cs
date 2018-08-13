using System.Runtime.Serialization;

namespace Ical.Net.CalendarComponents
{
    /// <summary>
    /// It represents an RFC 5545 VJOURNAL component.
    /// </summary>
    public class Journal : RecurringComponent
    {
        /// <summary>
        /// Constructs an Journal object, usually an <see cref="ICalendarObject"/>, as its parent.
        /// </summary>
        public Journal() : base(JournalStatus.Name){ }

        public string Status
        {
            get => Properties.Get<string>(JournalStatus.Key);
            set => Properties.Set(JournalStatus.Key, value);
        }

        protected override bool EvaluationIncludesReferenceDate => true;

        protected bool Equals(Journal other)
        {
            return Start.Equals(other.Start) && Equals(other as RecurringComponent);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Journal)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = Start?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ base.GetHashCode();
            return hashCode;
        }

        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);
        }
    }
}