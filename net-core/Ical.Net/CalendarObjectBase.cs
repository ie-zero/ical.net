using System;

namespace Ical.Net
{
    // TODO: CalendarObjectBase has a single descendant class (CalendarObject). Consider merging.

    public class CalendarObjectBase : ICopyable, ILoadable
    {
        public CalendarObjectBase()
        {
            IsLoaded = true;
        }

        public event EventHandler Loaded;

        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Creates a copy of the object.
        /// </summary>
        /// <returns>The copy of the object.</returns>
        public T Copy<T>()
        {
            var type = GetType();
            var obj = Activator.CreateInstance(type) as ICopyable;

            // Duplicate our values
            if (obj is T)
            {
                obj.CopyFrom(this);
                return (T)obj;
            }
            return default(T);
        }

        /// <summary>
        /// Copies values from the target object to the current object.
        /// </summary>
        public virtual void CopyFrom(ICopyable copyable) {}

        public void OnLoaded()
        {
            IsLoaded = true;
            Loaded?.Invoke(this, EventArgs.Empty);
        }
    }
}