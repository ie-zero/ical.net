using System;
using Ical.Net.Components;
using Ical.Net.DataTypes;
using NUnit.Framework;

namespace Ical.Net.CoreUnitTests
{
    public class ComponentTest
    {
        [Test, Category("Components")]
        public void UniqueComponent1()
        {
            var iCal = new Calendar();
            var evt = iCal.Create<CalendarEvent>();

            Assert.IsNotNull(evt.Uid);
            Assert.IsNull(evt.Created); // We don't want this to be set automatically
            Assert.IsNotNull(evt.DtStamp);
        }

        [Test, Category("Components")]
        public void ChangeCalDateTimeValue()
        {
            var e = new CalendarEvent
            {
                Start = new CalDateTime(2017, 11, 22, 11, 00, 01),
                End = new CalDateTime(2017, 11, 22, 11, 30, 01),
            };

            var firstStartAsUtc = e.Start.GetAsUtc();
            var firstEndAsUtc = e.End.GetAsUtc();

            e.Start.Value = new DateTime(2017, 11, 22, 11, 30, 01);
            e.End.Value = new DateTime(2017, 11, 22, 12, 00, 01);

            var secondStartAsUtc = e.Start.GetAsUtc();
            var secondEndAsUtc = e.End.GetAsUtc();

            Assert.AreNotEqual(firstStartAsUtc, secondStartAsUtc);
            Assert.AreNotEqual(firstEndAsUtc, secondEndAsUtc);
        }
    }
}
