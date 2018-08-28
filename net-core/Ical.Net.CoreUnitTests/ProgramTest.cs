using System;
using System.Linq;
using Ical.Net.DataTypes;
using Ical.Net.Utilities;
using NUnit.Framework;

namespace Ical.Net.CoreUnitTests
{
    [TestFixture]
    public class ProgramTest
    {
        [Test]
        public void LoadAndDisplayCalendar()
        {
            // The following code loads and displays an iCalendar
            // with US Holidays for 2006.
            var calendar = Calendar.Load(IcsFiles.UsHolidays);
            Assert.IsNotNull(calendar, "Calendar did not load.");
        }

        private const string _tzid = "US-Eastern";

        /// <summary>
        /// The following test is an aggregate of MonthlyCountByMonthDay3() and MonthlyByDay1() in the
        /// </summary>
        [Test]
        public void Merge1()
        {
            var calendar1 = Calendar.Load(IcsFiles.MonthlyCountByMonthDay3);
            var calendar2 = Calendar.Load(IcsFiles.MonthlyByDay1);

            // Change the UID of the 2nd event to make sure it's different
            calendar2.Events[calendar1.Events.First().Uid].Uid = "1234567890";
            calendar1.MergeWith(calendar2);

            var evt1 = calendar1.Events.First();
            var evt2 = calendar1.Events.Skip(1).First();

            // Get occurrences for the first event
            var occurrences = evt1.GetOccurrences(
                new CalDateTime(1996, 1, 1, _tzid),
                new CalDateTime(2000, 1, 1, _tzid)).OrderBy(o => o.Period.StartTime).ToList();

            var dateTimes = new[]
            {
                new CalDateTime(1997, 9, 10, 9, 0, 0, _tzid),
                new CalDateTime(1997, 9, 11, 9, 0, 0, _tzid),
                new CalDateTime(1997, 9, 12, 9, 0, 0, _tzid),
                new CalDateTime(1997, 9, 13, 9, 0, 0, _tzid),
                new CalDateTime(1997, 9, 14, 9, 0, 0, _tzid),
                new CalDateTime(1997, 9, 15, 9, 0, 0, _tzid),
                new CalDateTime(1999, 3, 10, 9, 0, 0, _tzid),
                new CalDateTime(1999, 3, 11, 9, 0, 0, _tzid),
                new CalDateTime(1999, 3, 12, 9, 0, 0, _tzid),
                new CalDateTime(1999, 3, 13, 9, 0, 0, _tzid)
            };

            var timeZones = new[]
            {
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern"
            };

            for (var i = 0; i < dateTimes.Length; i++)
            {
                IDateTime dt = dateTimes[i];
                var start = occurrences[i].Period.StartTime;
                Assert.AreEqual(dt, start);

                var expectedZone = DateUtilities.GetZone(dt.TimeZoneName);
                var actualZone = DateUtilities.GetZone(timeZones[i]);

                //Assert.AreEqual();

                //Normalize the time zones and then compare equality
                Assert.AreEqual(expectedZone, actualZone);

                //Assert.IsTrue(dt.TimeZoneName == TimeZones[i], "Event " + dt + " should occur in the " + TimeZones[i] + " timezone");
            }

            Assert.IsTrue(occurrences.Count == dateTimes.Length, "There should be exactly " + dateTimes.Length + " occurrences; there were " + occurrences.Count);

            // Get occurrences for the 2nd event
            occurrences = evt2.GetOccurrences(
                new CalDateTime(1996, 1, 1, _tzid),
                new CalDateTime(1998, 4, 1, _tzid)).OrderBy(o => o.Period.StartTime).ToList();

            var dateTimes1 = new[]
            {
                new CalDateTime(1997, 9, 2, 9, 0, 0, _tzid),
                new CalDateTime(1997, 9, 9, 9, 0, 0, _tzid),
                new CalDateTime(1997, 9, 16, 9, 0, 0, _tzid),
                new CalDateTime(1997, 9, 23, 9, 0, 0, _tzid),
                new CalDateTime(1997, 9, 30, 9, 0, 0, _tzid),
                new CalDateTime(1997, 11, 4, 9, 0, 0, _tzid),
                new CalDateTime(1997, 11, 11, 9, 0, 0, _tzid),
                new CalDateTime(1997, 11, 18, 9, 0, 0, _tzid),
                new CalDateTime(1997, 11, 25, 9, 0, 0, _tzid),
                new CalDateTime(1998, 1, 6, 9, 0, 0, _tzid),
                new CalDateTime(1998, 1, 13, 9, 0, 0, _tzid),
                new CalDateTime(1998, 1, 20, 9, 0, 0, _tzid),
                new CalDateTime(1998, 1, 27, 9, 0, 0, _tzid),
                new CalDateTime(1998, 3, 3, 9, 0, 0, _tzid),
                new CalDateTime(1998, 3, 10, 9, 0, 0, _tzid),
                new CalDateTime(1998, 3, 17, 9, 0, 0, _tzid),
                new CalDateTime(1998, 3, 24, 9, 0, 0, _tzid),
                new CalDateTime(1998, 3, 31, 9, 0, 0, _tzid)
            };

            var timeZones1 = new[]
            {
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",                
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern",
                "US-Eastern"
            };

            for (var i = 0; i < dateTimes1.Length; i++)
            {
                IDateTime dt = dateTimes1[i];
                var start = occurrences[i].Period.StartTime;
                Assert.AreEqual(dt, start);
                Assert.IsTrue(dt.TimeZoneName == timeZones1[i], "Event " + dt + " should occur in the " + timeZones1[i] + " timezone");
            }

            Assert.AreEqual(dateTimes1.Length, occurrences.Count, "There should be exactly " + dateTimes1.Length + " occurrences; there were " + occurrences.Count);
        }

        [Test]
        public void SystemTimeZone3()
        {
            // Per Jon Udell's test, we should be able to get all 
            // system time zones on the machine and ensure they
            // are properly translated.
            var zones = TimeZoneInfo.GetSystemTimeZones();
            foreach (var zone in zones)
            {
                try
                {
                    TimeZoneInfo.FindSystemTimeZoneById(zone.Id);                    
                }
                catch (Exception)
                {
                    Assert.Fail("Not found: " + zone.StandardName);                    
                }
            }
        }
    }
}
