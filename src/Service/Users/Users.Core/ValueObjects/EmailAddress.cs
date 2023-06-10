using System.Net.Mail;
using Users.Core.Exceptions;

namespace Users.Core.ValueObjects;

public sealed record EmailAddress
{
    public string Value { get; }
    private EmailAddress(){}

    public EmailAddress(string value)
    {
        try
        {
            var mail = new MailAddress(value);
        }
        catch (Exception e)
        {
            throw new InvalidEmailAddressException(value);
        }
        
        Value = value;
    }
    
    public static implicit operator EmailAddress(string date) => new (date);
}