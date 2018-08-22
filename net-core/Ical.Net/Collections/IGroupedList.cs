namespace Ical.Net.Collections
{
    public interface IGroupedList<T> : IGroupedCollection<T> where T : class, IGroupedObject
    {
    }
}
