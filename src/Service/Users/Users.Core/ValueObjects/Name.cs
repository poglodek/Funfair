namespace Users.Core.ValueObjects;

public record Name
{
    public string Value { get;}

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            
        }
        
        Value = value;
    }
    public static implicit operator Name(string date) => new (date);
};