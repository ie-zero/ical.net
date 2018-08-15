using System;

namespace Ical.Net.DataTypes
{
    public interface ICalendarDataType : ICalendarParameterCollectionContainer, IServiceProvider
    {
        ICalendarObject AssociatedObject { get; set; }
        Calendar Calendar { get; }
        string Language { get; set; }

        // TODO: GetValueType() and SetValueType() are candidates to be extracted in separate interface.
        Type GetValueType();
        void SetValueType(string type);
    }
}