using Ical.Net.DataTypes;
using NUnit.Framework;

namespace Ical.Net.CoreUnitTests
{
    [TestFixture]
    public class DataTypeTest
    {
        [Test, Category("DataType")]
        public void OrganizerConstructorMustAcceptNull()
        {
            Assert.DoesNotThrow(() => { new Organizer(null); });
        }

        [Test, Category("DataType")]
        public void AttachmentConstructorMustAcceptNull()
        {
            Assert.DoesNotThrow(() => { new Attachment((byte[])null); });
            Assert.DoesNotThrow(() => { new Attachment((string)null); });
        }
    }
}