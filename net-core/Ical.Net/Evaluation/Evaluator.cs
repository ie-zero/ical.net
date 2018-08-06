using System;
using System.Collections.Generic;
using System.Globalization;
using Ical.Net.DataTypes;
using Ical.Net.Utility;

namespace Ical.Net.Evaluation
{
    public abstract class Evaluator : IEvaluator
    {
        private readonly ICalendarDataType _associatedDataType;
        private ICalendarObject _associatedObject;

        protected Evaluator()
        {
            Calendar = CultureInfo.CurrentCulture.Calendar;

            Periods = new HashSet<Period>();
            EvaluationStartBounds = DateTime.MaxValue;
            EvaluationEndBounds = DateTime.MinValue;
        }

        protected Evaluator(ICalendarObject associatedObject) : this()
        {
            _associatedObject = associatedObject;
        }

        protected Evaluator(ICalendarDataType dataType) : this()
        {
            _associatedDataType = dataType;
        }

        public ICalendarObject AssociatedObject
        {
            get => _associatedObject ?? _associatedDataType?.AssociatedObject;
            protected set => _associatedObject = value;
        }

        public System.Globalization.Calendar Calendar { get; private set; }

        public HashSet<Period> Periods { get; private set; }
        public DateTime EvaluationEndBounds { get; protected set; } 
        public DateTime EvaluationStartBounds { get; protected set; }

        public virtual void Clear()
        {
            EvaluationStartBounds = DateTime.MaxValue;
            EvaluationEndBounds = DateTime.MinValue;
            Periods.Clear();
        }

        public abstract HashSet<Period> Evaluate(IDateTime referenceDate, DateTime periodStart, DateTime periodEnd, bool includeReferenceDateInResults);

        protected void IncrementDate(ref DateTime dt, RecurrencePattern pattern, int interval)
        {
            if (interval == 0)
            {
                throw new ArgumentException("Interval cannot be be zero", nameof(interval));
            }

            var old = dt;
            switch (pattern.Frequency)
            {
                case FrequencyType.Secondly:
                    dt = old.AddSeconds(interval);
                    break;
                case FrequencyType.Minutely:
                    dt = old.AddMinutes(interval);
                    break;
                case FrequencyType.Hourly:
                    dt = old.AddHours(interval);
                    break;
                case FrequencyType.Daily:
                    dt = old.AddDays(interval);
                    break;
                case FrequencyType.Weekly:
                    dt = DateUtil.AddWeeks(old, interval, pattern.FirstDayOfWeek);
                    break;
                case FrequencyType.Monthly:
                    dt = old.AddDays(-old.Day + 1).AddMonths(interval);
                    break;
                case FrequencyType.Yearly:
                    dt = old.AddDays(-old.DayOfYear + 1).AddYears(interval);
                    break;
                default:
                    throw new InvalidOperationException($"Invalid FrequencyType ({nameof(pattern.Frequency)}). Specify a valid FrequencyType before evaluating the recurrence.");
            }
        }
    }
}