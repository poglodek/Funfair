
using Users.Core.Exceptions;

namespace Users.Core.ValueObjects;

public record Date
{
    public Date(DateTime value)
    {
        if (value == default || value == DateTime.MinValue || value == DateTime.MaxValue)
        {
            throw new InvalidDateException(value);
        }
        Value = value;
    }

    public DateTime Value { get; }
    public static implicit operator Date(DateTime date) => new (date);
};