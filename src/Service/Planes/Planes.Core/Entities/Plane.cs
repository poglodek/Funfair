using Funfair.Shared.Domain;
using Planes.Core.Events;
using Planes.Core.Exceptions;
using Planes.Core.ValueObjects;

namespace Planes.Core.Entities;

public sealed class Plane : DomainBase
{
    private readonly List<Seat> _seats = new();
    public IReadOnlyCollection<Seat> Seats 
        => _seats.ToList().AsReadOnly();

    public Model Model { get; private set; }
    public ProductionYear ProductionYear { get; private set; }

    private Plane()
    { }

    private Plane(Id id, Model model, ProductionYear year, IEnumerable<Seat> seats)
    {
        if (id.Value == Guid.Empty)
        {
            throw new InvalidIdException(id);
        }

        if (string.IsNullOrWhiteSpace(model.Value))
        {
            throw new InvalidPlaneNameException(model.Value);
        }
        
        if(seats is null || !seats.Any())
        {
            throw new InvalidSeatsException("Seats cannot be empty");
        }

        if (year.Year < 2000)
        {
            throw new InvalidProductionYearException(year.Year);
        }
        
        Id = id;
        Model = model;
        ProductionYear = year;
        _seats.Clear();
        _seats.AddRange(seats);
        
    }


    public static Plane Create(Id id, Model model, ProductionYear year, IEnumerable<Seat> seats)
    {
        var plane = new Plane(id, model, year, seats);
        
        plane.RaiseEvent(new NewPlaneCreated(plane.Id, plane.Model, plane.Seats));

        return plane;
    }

}
