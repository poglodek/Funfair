using System.Net.Mail;
using Users.Core.Exceptions;

namespace Users.Core.ValueObjects;

public record EmailAddress
{
    public string Value { get; }

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
}