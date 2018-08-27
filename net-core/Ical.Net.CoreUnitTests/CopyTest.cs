using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ical.Net.Components;
using Ical.Net.DataTypes;
using Ical.Net.Tests.Support;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Ical.Net.CoreUnitTests
{
    [TestFixture]
    public class CopyTest
    {
        [Test, TestCaseSource(nameof(CopyCalendarTest_TestCases)), Category("Copy tests")]
        public void CopiedCalendarShouldBeTheSameToTheOriginal(string calendarString)
        {
            // Arrange
            var calendar = Calendar.Load(calendarString);

            // Act
            var calendarCopy = calendar.Copy();

            // Assert
            SerializationTests.CompareCalendars(calendar, calendarCopy);
        }

        public static IEnumerable<ITestCaseData> CopyCalendarTest_TestCases()
        {
            yield return new TestCaseData(IcsFiles.Attachment3).SetName("Attachment3");
            yield return new TestCaseData(IcsFiles.Bug2148092).SetName("Bug2148092");
            yield return new TestCaseData(IcsFiles.CaseInsensitive1).SetName("CaseInsensitive1");
            yield return new TestCaseData(IcsFiles.CaseInsensitive2).SetName("CaseInsensitive2");
            yield return new TestCaseData(IcsFiles.CaseInsensitive3).SetName("CaseInsensitive3");
            yield return new TestCaseData(IcsFiles.Categories1).SetName("Categories1");
            yield return new TestCaseData(IcsFiles.Duration1).SetName("Duration1");
            yield return new TestCaseData(IcsFiles.Encoding1).SetName("Encoding1");
            yield return new TestCaseData(IcsFiles.Event1).SetName("Event1");
            yield return new TestCaseData(IcsFiles.Event2).SetName("Event2");
            yield return new TestCaseData(IcsFiles.Event3).SetName("Event3");
            yield return new TestCaseData(IcsFiles.Event4).SetName("Event4");
            yield return new TestCaseData(IcsFiles.GeographicLocation1).SetName("GeographicLocation1");
            yield return new TestCaseData(IcsFiles.Language1).SetName("Language1");
            yield return new TestCaseData(IcsFiles.Language2).SetName("Language2");
            yield return new TestCaseData(IcsFiles.Language3).SetName("Language3");
            yield return new TestCaseData(IcsFiles.TimeZone1).SetName("TimeZone1");
            yield return new TestCaseData(IcsFiles.TimeZone2).SetName("TimeZone2");
            yield return new TestCaseData(IcsFiles.TimeZone3).SetName("TimeZone3");
            yield return new TestCaseData(IcsFiles.XProperty1).SetName("XProperty1");
            yield return new TestCaseData(IcsFiles.XProperty2).SetName("XProperty2");
        }

        [Test]
        public void CopyCreatesAShallowCopyOfTheOriginal()
        {
            // TODO: Copy() method creates a shallow copy of the original component. Is this the intention?

            // Arrange + Act
            var calendarEvent = CreateSimpleEvent();
            calendarEvent.Uid = "Hello";
            var copy = calendarEvent.Copy();
            copy.Uid = "Goodbye";

            // Assert
            Assert.AreEqual(calendarEvent.Uid, copy.Uid);
        }

        [Test]
        public void OriginalAndCopyComponentsShouldHaveIdenticalUid()
        {
            // TODO: This should be valid for all UniqueComponents

            // Arrange
            var calendarEvent = CreateSimpleEvent();
            calendarEvent.Uid = "Hello";

            // Act
            var copy = calendarEvent.Copy();

            // Assert
            Assert.AreEqual(calendarEvent.Uid, copy.Uid);
        }

        private static CalendarEvent CreateSimpleEvent()
        {
            return new CalendarEvent
            {
                DtStart = new CalDateTime(2018, 10, 12, 14, 00, 00),
                DtEnd = new CalDateTime(2018, 10, 12, 15, 00, 00),
                Duration = TimeSpan.FromHours(1),
            };
        }
    }
}
