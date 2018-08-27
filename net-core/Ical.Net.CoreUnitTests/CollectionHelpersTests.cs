using System;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.DataTypes;
using NUnit.Framework;

namespace Ical.Net.CoreUnitTests
{
    public class CollectionHelpersTests
    {
        [Test]
        public void ExDateTests()
        {
            // TODO: It is still unclear what the purpose of this test actually is.

            IEnumerable<PeriodList> changedPeriod = CreateExceptionDates();
            changedPeriod.First().First().StartTime = new CalDateTime(2018, 10, 12).AddHours(-1);

            Assert.AreNotEqual(CreateExceptionDates(), changedPeriod);
        }

        private static IEnumerable<PeriodList> CreateExceptionDates()
        {
            return new PeriodList[] { new PeriodList { new Period(new CalDateTime(2018, 10, 12)) } };
        }
    }
}
