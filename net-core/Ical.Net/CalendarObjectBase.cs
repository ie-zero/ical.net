using System;

namespace Ical.Net
{
    // TODO: CalendarObjectBase has a single descendant class (CalendarObject). Consider merging.

    public class CalendarObjectBase : ICopyable, ILoadable
    {
        private bool _isLoaded;

        public CalendarObjectBase()
        {
            _isLoaded = true;
        }

        public event EventHandler Loaded;

        public virtual bool IsLoaded => _isLoaded;

        /// <summary>
        /// Creates a copy of the object.
        /// </summary>
        /// <returns>The copy of the object.</returns>
        public virtual T Copy<T>()
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

        public virtual void OnLoaded()
        {
            _isLoaded = true;
            Loaded?.Invoke(this, EventArgs.Empty);
        }
    }
}