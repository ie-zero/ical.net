using System;
using System.Collections.Generic;
using Ical.Net.DataTypes;

namespace Ical.Net.Evaluation
{
    public class PeriodListEvaluator : Evaluator
    {
        private readonly PeriodList _periodList;

        public PeriodListEvaluator(PeriodList rdt)
        {
            _periodList = rdt;
        }

        public override HashSet<Period> Evaluate(IDateTime referenceDate, DateTime periodStart, DateTime periodEnd, bool includeReferenceDateInResults)
        {
            var periods = new HashSet<Period>();

            if (includeReferenceDateInResults)
            {
                var p = new Period(referenceDate);
                periods.Add(p);
            }

            if (periodEnd < periodStart)
            {
                return periods;
            }

            periods.UnionWith(_periodList);
            return periods;
        }
    }
}