namespace Funfair.Shared.Core;

public interface IClock
{
    public DateTime CurrentDateTime { get; }
}

public class ClockNow : IClock
{
    public DateTime CurrentDateTime => DateTime.UtcNow;
}