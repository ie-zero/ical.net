namespace Ical.Net.Collections
{
    public interface IGroupedObject<TKey>
    {
        TKey Group { get; set; }
    }
}
