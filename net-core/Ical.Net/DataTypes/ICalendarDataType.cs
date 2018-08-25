﻿using System;

namespace Ical.Net.DataTypes
{
    public interface ICalendarDataType : IPropertyParameters
    {
        ICalendarObject AssociatedObject { get; }
        Calendar Calendar { get; }
        string Language { get; set; }

        void Associate(ICalendarObject associatedObject);

        // TODO: GetValueType() and SetValueType() are candidates to be extracted in separate interface.
        Type GetValueType();
        void SetValueType(string type);
    }
}