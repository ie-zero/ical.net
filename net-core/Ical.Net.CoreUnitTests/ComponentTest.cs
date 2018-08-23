using System;
using Ical.Net.Components;
using Ical.Net.DataTypes;
using NUnit.Framework;

namespace Ical.Net.CoreUnitTests
{
    public class ComponentTest
    {
        [Test, Category("Components")]
        public void VerifyUniqueComponentPropertiesOnCreate()
        {
            var calendar = new Calendar();
            var component = calendar.Create<DummyUniqueComponent>();

            Assert.IsNotNull(component.Uid);
            Assert.IsNotNull(component.DtStamp);
        }

        private class DummyUniqueComponent : UniqueComponent
        {
            public DummyUniqueComponent() : base(name: "DUMMY") {}
        }

        [Test, Category("Components")]
        public void ChangeCalDateTimeValue()
        {
            // TODO: Review this test. It does not appear to be testing anything of value. 

            var evt = new CalendarEvent
            {
                Start = new CalDateTime(2017, 11, 22, 11, 00, 01),
                End = new CalDateTime(2017, 11, 22, 11, 30, 01),
            };

            var firstStartAsUtc = evt.Start.GetAsUtc();
            var firstEndAsUtc = evt.End.GetAsUtc();

            evt.Start.Value = new DateTime(2017, 11, 22, 11, 30, 01);
            evt.End.Value = new DateTime(2017, 11, 22, 12, 00, 01);

            var secondStartAsUtc = evt.Start.GetAsUtc();
            var secondEndAsUtc = evt.End.GetAsUtc();

            Assert.AreNotEqual(firstStartAsUtc, secondStartAsUtc);
            Assert.AreNotEqual(firstEndAsUtc, secondEndAsUtc);
        }
    }
}
