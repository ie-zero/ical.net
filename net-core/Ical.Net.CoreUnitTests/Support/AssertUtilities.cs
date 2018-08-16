using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ical.Net.CoreUnitTests.Support
{
    internal class AssertUtilities
    {
        public static void AssertCalendarHasComponents(Calendar calendar)
        {
            Assert.IsNotNull(calendar, "The iCalendar was not loaded");

            if (calendar.Events.Count > 0)
                Assert.IsTrue(calendar.Events.Count == 1, $"Calendar should contain 1 event; however, the iCalendar loaded {calendar.Events.Count} events");

            if (calendar.Todos.Count > 0)
                Assert.IsTrue(calendar.Todos.Count == 1, $"Calendar should contain 1 todo; however, the iCalendar loaded {calendar.Todos.Count} todos");
        }
    }
}
