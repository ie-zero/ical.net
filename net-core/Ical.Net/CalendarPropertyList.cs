using System.Linq;
using Ical.Net.Collections;

namespace Ical.Net
{
    public class CalendarPropertyList : GroupedValueList<CalendarProperty, object>
    {
        private readonly ICalendarObject _parent;

        public CalendarPropertyList(ICalendarObject parent)
        {
            _parent = parent;
            ItemAdded += CalendarPropertyList_ItemAdded;
        }

        private void CalendarPropertyList_ItemAdded(object sender, ItemProcessedEventArgs<CalendarProperty> e)
        {
            e.Item.Parent = _parent;
        }

        public ICalendarProperty this[string name]
        {
            get
            {
                return ContainsKey(name) ? AllOf(name).FirstOrDefault() : null;
            }
        }
    }
}