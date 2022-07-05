using ImapCleanup.Extensions;

namespace ImapCleanup.Tests
{
    public class TimeSpanExtensionsTests
    {
        [Fact]
        public void When_in_timeframe_true_is_returned()
        {
            var sut = TimeSpan.Parse("13:37");
            Assert.True(sut.IsInTimeFrame(new TimeSpan(12, 00, 00), new TimeSpan(14,00,00)));
        }

        [Fact]
        public void When_before_timeframe_false_is_returned()
        {
            var sut = TimeSpan.Parse("11:37");
            Assert.False(sut.IsInTimeFrame(new TimeSpan(12, 00, 00), new TimeSpan(14, 00, 00)));
        }

        [Fact]
        public void When_after_timeframe_false_is_returned()
        {
            var sut = TimeSpan.Parse("11:37");
            Assert.False(sut.IsInTimeFrame(new TimeSpan(12, 00, 00), new TimeSpan(14, 00, 00)));
        }

        [Fact]
        public void When_on_fromTime_true_is_returned()
        {
            var sut = TimeSpan.Parse("12:00");
            Assert.True(sut.IsInTimeFrame(new TimeSpan(12, 00, 00), new TimeSpan(14, 00, 00)));
        }

        [Fact]
        public void When_on_toTime_true_is_returned()
        {
            var sut = TimeSpan.Parse("14:00");
            Assert.True(sut.IsInTimeFrame(new TimeSpan(12, 00, 00), new TimeSpan(14, 00, 00)));
        }

        [Fact]
        public void When_timeframe_spans_days_and_in_timeframe_true_is_returned()
        {
            var sut = TimeSpan.Parse("23:00");
            Assert.True(sut.IsInTimeFrame(new TimeSpan(22, 00, 00), new TimeSpan(6, 00, 00)));
        }

        [Fact]
        public void When_timeframe_spans_days_and_in_timeframe_next_day_true_is_returned()
        {
            var sut = TimeSpan.Parse("5:00");
            Assert.True(sut.IsInTimeFrame(new TimeSpan(22, 00, 00), new TimeSpan(6, 00, 00)));
        }

        [Fact]
        public void When_timeframe_spans_days_and_before_timeframe_false_is_returned()
        {
            var sut = TimeSpan.Parse("21:00");
            Assert.False(sut.IsInTimeFrame(new TimeSpan(22, 00, 00), new TimeSpan(6, 00, 00)));
        }

        [Fact]
        public void When_timeframe_spans_days_and_after_timeframe_false_is_returned()
        {
            var sut = TimeSpan.Parse("6:01");
            Assert.False(sut.IsInTimeFrame(new TimeSpan(22, 00, 00), new TimeSpan(6, 00, 00)));
        }
    }
}