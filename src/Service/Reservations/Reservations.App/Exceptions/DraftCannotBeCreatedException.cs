using Funfair.Shared.App;

namespace Reservations.App.Exceptions;

public class DraftCannotBeCreatedException(string value) : AppException(value)
{
    public override string ErrorMessage => "draft_cannot_be_created";
}