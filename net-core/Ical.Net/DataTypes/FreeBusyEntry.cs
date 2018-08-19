namespace Ical.Net.DataTypes
{
    public class FreeBusyEntry : Period
    {
        // TODO: Consider removing the default .ctor
        public FreeBusyEntry()
        {
            Status = FreeBusyStatus.Busy;
        }

        public FreeBusyEntry(Period period, FreeBusyStatus status)
        {
            // TODO: Sets the status associated with a given period, which requires copying the period values
            //      Probably the Period object should just have a FreeBusyStatus directly?
            CopyFrom(period);
            Status = status;
        }

        public FreeBusyStatus Status { get; set; }

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            var freeBusy = copyable as FreeBusyEntry;
            if (freeBusy == null) { return; }

            CopyFrom(freeBusy);
        }

        private void CopyFrom(FreeBusyEntry freeBusy)
        {
            Status = freeBusy.Status;
        }
    }
}