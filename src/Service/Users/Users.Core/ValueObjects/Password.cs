using Users.Core.Exceptions;

namespace Users.Core.ValueObjects;

public record Password
{
    public string Value { get; }

    public Password(string value)
    {
        if(string.IsNullOrWhiteSpace(value) || value.Length < 2)
        {
            throw new InvalidPasswordException(value);
        }
        
        Value = value;
    }
    public static implicit operator Password(string date) => new (date);
}