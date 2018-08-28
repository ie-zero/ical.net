using System;
using System.Linq;
using Ical.Net.CoreUnitTests.Support;
using NUnit.Framework;

namespace Ical.Net.CoreUnitTests
{
    [TestFixture]
    public class JournalTest
    {
        [Test, Category("Journal")]
        public void Journal1()
        {
            var calendar = Calendar.Load(IcsFiles.Journal1);
            AssertUtilities.AssertCalendarHasComponents(calendar);
            Assert.AreEqual(1, calendar.Journals.Count);
            var journal = calendar.Journals.First();

            Assert.IsNotNull(journal, "Journal entry was null");
            Assert.AreEqual(JournalStatus.Draft, journal.Status, "Journal entry should have been in DRAFT status, but it was in " + journal.Status + " status.");
            Assert.AreEqual("PUBLIC", journal.Class, "Journal class should have been PUBLIC, but was " + journal.Class + ".");
            Assert.IsNull(journal.Start);
        }

        [Test, Category("Journal")]
        public void Journal2()
        {
            var calendar = Calendar.Load(IcsFiles.Journal2);
            AssertUtilities.AssertCalendarHasComponents(calendar);
            Assert.AreEqual(1, calendar.Journals.Count);
            var journal = calendar.Journals.First();

            Assert.IsNotNull(journal, "Journal entry was null");
            Assert.AreEqual(JournalStatus.Final, journal.Status, "Journal entry should have been in FINAL status, but it was in " + journal.Status + " status.");
            Assert.AreEqual("PRIVATE", journal.Class, "Journal class should have been PRIVATE, but was " + journal.Class + ".");
            Assert.AreEqual("JohnSmith", journal.Organizer.CommonName, "Organizer common name should have been JohnSmith, but was " + journal.Organizer.CommonName);
            Assert.IsTrue(
                string.Equals(
                    journal.Organizer.SentBy.OriginalString,
                    "mailto:jane_doe@host.com",
                    StringComparison.OrdinalIgnoreCase),
                "Organizer should have had been SENT-BY 'mailto:jane_doe@host.com'; it was sent by '" + journal.Organizer.SentBy + "'");
            Assert.IsTrue(
                string.Equals(
                    journal.Organizer.DirectoryEntry.OriginalString,
                    "ldap://host.com:6666/o=3DDC%20Associates,c=3DUS??(cn=3DJohn%20Smith)",
                    StringComparison.OrdinalIgnoreCase),
                "Organizer's directory entry should have been 'ldap://host.com:6666/o=3DDC%20Associates,c=3DUS??(cn=3DJohn%20Smith)', but it was '" + journal.Organizer.DirectoryEntry + "'");
            Assert.AreEqual(
                "MAILTO:jsmith@host.com",
                journal.Organizer.Value.OriginalString);
            Assert.AreEqual(
                "jsmith",
                journal.Organizer.Value.UserInfo);
            Assert.AreEqual(
                "host.com",
                journal.Organizer.Value.Host);
            Assert.IsNull(journal.Start);
        }
    }
}
