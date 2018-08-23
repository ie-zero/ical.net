using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ical.Net.Evaluation;
using Ical.Net.Serialization;
using Ical.Net.Serialization.DataTypes;
using Ical.Net.Utilities;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// An iCalendar list of recurring dates (or date exclusions)
    /// </summary>
    public class PeriodList : EncodableDataType, IEnumerable<Period>
    {
        public PeriodList()
        {
            Periods = new List<Period>();
            SetService(new PeriodListEvaluator(this));
        }

        public PeriodList(string value) : this()
        {
            var serializer = new PeriodListSerializer(SerializationContext.Default);
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        public Period this[int index]
        {
            get { return Periods[index]; }
        }

        public int Count
        {
            get { return Periods.Count; }
        }

        public string TzId { get; set; }

        protected IList<Period> Periods { get; }

        public void Clear()
        {
            Periods.Clear();
        }

        public bool Contains(Period item)
        {
            return Periods.Contains(item);
        }

        public void Add(IDateTime dt)
        {
            Periods.Add(new Period(dt));
        }

        public void Add(Period item)
        {
            Periods.Add(item);
        }

        public bool Remove(Period item)
        {
            return Periods.Remove(item);
        }

        public static IDictionary<string, IEnumerable<Period>> GetGroupedPeriods(IEnumerable<PeriodList> periodLists)
        {
            // In order to know if two events are equal, a semantic understanding of exdates, rdates, rrules, and exrules is required. This could be done by
            // computing the complete recurrence set (expensive) while being time-zone sensitive, or by comparing each List<Period> in each IPeriodList.

            // For example, events containing these rules generate the same recurrence set, including having the same time zone for each occurrence, so
            // they're the same:
            //
            // Event A:
            // RDATE:20170302T060000Z,20170303T060000Z
            //
            // Event B:
            // RDATE:20170302T060000Z
            // RDATE:20170303T060000Z

            var grouped = new Dictionary<string, HashSet<Period>>(StringComparer.OrdinalIgnoreCase);
            foreach (var periodList in periodLists)
            {
                var defaultBucket = string.IsNullOrWhiteSpace(periodList.TzId) ? "" : periodList.TzId;

                foreach (var period in periodList)
                {
                    var actualBucket = string.IsNullOrWhiteSpace(period.StartTime.TzId) ? defaultBucket : period.StartTime.TzId;

                    if (!grouped.ContainsKey(actualBucket))
                    {
                        grouped.Add(actualBucket, new HashSet<Period>());
                    }
                    grouped[actualBucket].Add(period);
                }
            }
            return grouped.ToDictionary(k => k.Key, v => v.Value.OrderBy(d => d.StartTime).AsEnumerable());
        }

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            var periods = copyable as PeriodList;
            if (periods == null) { return; }

            CopyFrom(periods);
        }

        private void CopyFrom(PeriodList periods)
        {
            periods.Clear();
            foreach (var period in periods)
            {
                Add(period);
            }
        }

        public IEnumerator<Period> GetEnumerator()
        {
            return Periods.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Periods.GetEnumerator();
        }

        public override string ToString()
        {
            return new PeriodListSerializer(SerializationContext.Default).SerializeToString(this);
        }

        protected bool Equals(PeriodList other)
        {
            return string.Equals(TzId, other.TzId, StringComparison.OrdinalIgnoreCase)
                && CollectionHelpers.Equals(Periods, other.Periods);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((PeriodList)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TzId?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ CollectionHelpers.GetHashCode(Periods);
                return hashCode;
            }
        }
    }
}
