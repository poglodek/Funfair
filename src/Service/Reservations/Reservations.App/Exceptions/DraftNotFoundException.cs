using Funfair.Shared.App;

namespace Reservations.App.Exceptions;

public class DraftNotFoundException(Guid id) : AppException($"Draft with id {id} not found")
{
    public override string ErrorMessage => "draft_not_found";
}