using Funfair.Shared.Core;

namespace Test.Shared;

public class ClockTest(DateTime time) : IClock
{
    public DateTimeOffset CurrentDateTime { get; } = time;

    public ClockTest() : this(DateTime.Now)
    { }

    public ClockTest(string date) : this(DateTime.Parse(date))
    { }
}