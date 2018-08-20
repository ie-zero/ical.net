using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Ical.Net.DataTypes;
using Ical.Net.Utility;

namespace Ical.Net.Components
{
    /// <summary>
    /// Represents a unique component, a component with a unique UID, which can be used to uniquely
    /// identify the component.
    /// </summary>
    public abstract class UniqueComponent : CalendarComponent, IUniqueComponent, IComparable<UniqueComponent>
    {
        // TODO: Add AddRelationship() public method.
        //      This method will add the UID of a related component to the Related_To property, 
        //      along with any "RELTYPE" parameter ("PARENT", "CHILD", "SIBLING", or other)

        // TODO: Add RemoveRelationship() public method.        

        public UniqueComponent()
        {
            EnsureProperties();
        }

        public UniqueComponent(string name) : base(name)
        {
            EnsureProperties();
        }

        public IList<Attendee> Attendees
        {
            get => Properties.GetMany<Attendee>("ATTENDEE");
            set => Properties.Set("ATTENDEE", value);
        }

        public IList<string> Comments
        {
            get => Properties.GetMany<string>("COMMENT");
            set => Properties.Set("COMMENT", value);
        }

        public IDateTime DtStamp
        {
            get => Properties.Get<IDateTime>("DTSTAMP");
            set => Properties.Set("DTSTAMP", value);
        }

        public Organizer Organizer
        {
            get => Properties.Get<Organizer>("ORGANIZER");
            set => Properties.Set("ORGANIZER", value);
        }

        public IList<RequestStatus> RequestStatuses
        {
            get => Properties.GetMany<RequestStatus>("REQUEST-STATUS");
            set => Properties.Set("REQUEST-STATUS", value);
        }

        public string Uid
        {
            get => Properties.Get<string>("UID");
            set => Properties.Set("UID", value);
        }

        public Uri Url
        {
            get => Properties.Get<Uri>("URL");
            set => Properties.Set("URL", value);
        }

        public int CompareTo(UniqueComponent other)
        {
            return string.Compare(Uid, other.Uid, StringComparison.OrdinalIgnoreCase);
        }

        protected override void OnDeserialized(StreamingContext context)
        {
            base.OnDeserialized(context);

            EnsureProperties();
        }

        private void EnsureProperties()
        {
            CreateUidIfEmpty();
            CreateTimestampIfEmpty();
        }

        private void CreateUidIfEmpty()
        {
            if (string.IsNullOrEmpty(Uid))
            {
                // Create a new UID for the component
                Uid = Guid.NewGuid().ToString();
            }
        }

        private void CreateTimestampIfEmpty()
        {
            // NOTE: removed setting the 'CREATED' property here since it breaks serialization.
            // See https://sourceforge.net/projects/dday-ical/forums/forum/656447/topic/3754354
            if (DtStamp == null)
            {
                // TODO: Hard-coded DateTime component.

                // iCalendar RFC doesn't care about sub-second time resolution, so shave off everything smaller than seconds.
                var utcNow = DateTime.UtcNow.Truncate(TimeSpan.FromSeconds(1));
                DtStamp = new CalDateTime(utcNow, "UTC");
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }

            var uniqueComponent = (UniqueComponent)obj;

            return GetEqualityComponents().SequenceEqual(uniqueComponent.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        protected IEnumerable<object> GetEqualityComponents()
        {
            yield return Uid;
        }
    }
}