namespace Funfair.Shared.Core;

public interface IClock
{
    public DateTime CurrentDateTime { get; }
}