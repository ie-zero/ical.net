﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Ical.Net.CoreUnitTests
{
    public class ConcurrentDeserializationTests
    {
        [Test]
        public void ConcurrentDeserialization_Test()
        {
            // https://github.com/rianjs/ical.net/issues/40
            var calendars = new List<string>
            {
                IcsFiles.DailyCount2,
                IcsFiles.DailyInterval2,
                IcsFiles.DailyByDay1,
                IcsFiles.RecurrenceDates1,
                IcsFiles.DailyByHourMinute1,
            };

            var deserializedCalendars = calendars.AsParallel().SelectMany(Calendar.LoadMany);
            var materialized = deserializedCalendars.ToList();
            Assert.AreEqual(5, materialized.Count);
        }
    }
}
