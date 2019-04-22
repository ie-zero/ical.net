namespace Ical.Net
{
    public interface IFileLocation
    {
        /// <summary>
        /// Returns the line number, within the file, where a calendar object was found during parsing.
        /// </summary>
        int Line { get; }

        /// <summary>
        /// Returns the column number, within the file, where a calendar object was found during parsing.
        /// </summary>
        int Column { get; }
    }
}