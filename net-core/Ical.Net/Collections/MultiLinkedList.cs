using System;
using System.Collections.Generic;

namespace Ical.Net.Collections
{
    public class MultiLinkedList<T> : List<T>
    {
        public int StartIndex => 0;

        public int ExclusiveEnd
        {
            get { return Count > 0 ? Count : 0; }
        }
    }
}
