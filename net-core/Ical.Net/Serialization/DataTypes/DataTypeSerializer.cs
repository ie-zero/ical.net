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

            dt.Associate(SerializationContext.Peek() as ICalendarObject);
            return dt;
        }

        private ICalendarDataType Create()
        {
            return Activator.CreateInstance(TargetType) as ICalendarDataType;
        }
    }
}