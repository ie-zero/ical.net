using System;
using BenchmarkDotNet.Attributes;
using Ical.Net.DataTypes;

namespace PerfTests
{
    public class CalDateTimePerfTests
    {
        private const string _aTzid = "Australia/Sydney";
        private const string _bTzid = "America/New_York";

        [Benchmark]
        public CalDateTime EmptyTzid() => new CalDateTime(DateTime.Now);

        [Benchmark]
        public CalDateTime SpecifiedTzid() => new CalDateTime(DateTime.Now, _aTzid);

        [Benchmark]
        public CalDateTime UtcDateTime() => new CalDateTime(DateTime.UtcNow);

        [Benchmark]
        public CalDateTime EmptyTzidToTzid() => EmptyTzid().ToTimeZone(_bTzid);

        [Benchmark]
        public CalDateTime SpecifiedTzidToDifferentTzid() => SpecifiedTzid().ToTimeZone(_bTzid);

        [Benchmark]
        public CalDateTime UtcToDifferentTzid() => UtcDateTime().ToTimeZone(_bTzid);
    }
}