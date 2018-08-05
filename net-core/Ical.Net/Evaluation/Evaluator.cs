using System;
using System.Collections.Generic;
using System.Globalization;
using Ical.Net.DataTypes;
using Ical.Net.Utility;

namespace Ical.Net.Evaluation
{
    public abstract class Evaluator : IEvaluator
    {
        // TODO: Fix 'MPeriods' name.
        protected HashSet<Period> MPeriods;
        private readonly ICalendarDataType _associatedDataType;
        private ICalendarObject _associatedObject;
        private DateTime _evaluationEndBounds = DateTime.MinValue;
        private DateTime _evaluationStartBounds = DateTime.MaxValue;

        protected Evaluator()
        {
            Initialize();
        }

        protected Evaluator(ICalendarObject associatedObject)
        {
            _associatedObject = associatedObject;

            Initialize();
        }

        protected Evaluator(ICalendarDataType dataType)
        {
            _associatedDataType = dataType;

            Initialize();
        }

        public ICalendarObject AssociatedObject
        {
            get => _associatedObject ?? _associatedDataType?.AssociatedObject;
            protected set => _associatedObject = value;
        }

        public System.Globalization.Calendar Calendar { get; private set; }

        public DateTime EvaluationEndBounds
        {
            get => _evaluationEndBounds;
            set => _evaluationEndBounds = value;
        }

        public DateTime EvaluationStartBounds
        {
            get => _evaluationStartBounds;
            set => _evaluationStartBounds = value;
        }

        public HashSet<Period> Periods => MPeriods;

        public virtual void Clear()
        {
            _evaluationStartBounds = DateTime.MaxValue;
            _evaluationEndBounds = DateTime.MinValue;
            MPeriods.Clear();
        }

        public abstract HashSet<Period> Evaluate(IDateTime referenceDate, DateTime periodStart, DateTime periodEnd, bool includeReferenceDateInResults);

        protected IDateTime ConvertToIDateTime(DateTime dt, IDateTime referenceDate)
        {
            IDateTime newDt = new CalDateTime(dt, referenceDate.TzId);
            newDt.AssociateWith(referenceDate);
            return newDt;
        }

        protected void IncrementDate(ref DateTime dt, RecurrencePattern pattern, int interval)
        {
            if (interval == 0)
            {
                // TODO: Use more specific exception
                throw new Exception("Cannot evaluate with an interval of zero.  Please use an interval other than zero.");
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
                    // TODO: Use more specific exception
                    throw new Exception("FrequencyType.NONE cannot be evaluated. Please specify a FrequencyType before evaluating the recurrence.");
            }
        }

        private void Initialize()
        {
            Calendar = CultureInfo.CurrentCulture.Calendar;
            MPeriods = new HashSet<Period>();
        }
    }
}