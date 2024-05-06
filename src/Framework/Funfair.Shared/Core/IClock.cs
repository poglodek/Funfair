namespace Funfair.Shared.Core;

public interface IClock
{
    public DateTimeOffset CurrentDateTime { get; }
}

public class ClockNow : IClock
{
    public DateTimeOffset CurrentDateTime => DateTimeOffset.UtcNow;
}