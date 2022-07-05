using System;

namespace ImapCleanup.Extensions
{
    /// <summary>
    /// TimeSpan extension methods.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Checks if a given time is withing a given time range.
        /// </summary>
        /// <param name="time">Time to check.</param>
        /// <param name="from">From time of the range.</param>
        /// <param name="to">To time of the range.</param>
        /// <returns></returns>
        public static bool IsInTimeFrame(this TimeSpan time, TimeSpan from, TimeSpan to)
        {
            var fromTimeMinutes = from.TotalMinutes;
            var toTimeMinutes = to.TotalMinutes;

            var checkTimeMinutes = time.TotalMinutes;

            if (fromTimeMinutes <= toTimeMinutes)
            {
                // Normal case, where both times are inbetween 0:00 and 23:59 of the same day.
                return checkTimeMinutes >= fromTimeMinutes && checkTimeMinutes <= toTimeMinutes;
            }
            else
            {
                // To-time is lower than the from-time, so it spans to the next day (like 22:00 to 02:00).
                return checkTimeMinutes >= fromTimeMinutes ? true : checkTimeMinutes <= toTimeMinutes;
            }
        }
    }
}
