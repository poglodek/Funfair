using Funfair.Shared.Core;

namespace Test.Shared;

public class ClockTest(DateTime time) : IClock
{
    public DateTime CurrentDateTime { get; } = time;

    public ClockTest() : this(DateTime.Now)
    {
    }
}