using System;
using Ical.Net.DataTypes;

namespace Ical.Net.Serialization.DataTypes
{
    public abstract class DataTypeSerializer : SerializerBase
    {
        protected DataTypeSerializer(SerializationContext ctx) : base(ctx) {}

        protected ICalendarDataType CreateAndAssociate()
        {
            ICalendarDataType dt = Create();
            if (dt == null) { return null; }

            return Associate(dt, SerializationContext.Peek() as ICalendarObject);
        }

        private ICalendarDataType Create()
        {
            return Activator.CreateInstance(TargetType) as ICalendarDataType;
        }

        private ICalendarDataType Associate(ICalendarDataType dt, ICalendarObject associatedObject)
        {
            if (associatedObject != null)
            {
                dt.AssociatedObject = associatedObject;
            }

            return dt;
        }
    }
}