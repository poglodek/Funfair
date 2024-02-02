using Planes.Core.Entities;
using Planes.Core.Exceptions;
using Planes.Core.ValueObjects;

namespace Planes.Core.Builder;

public sealed class PlaneBuilder
{
    private const string AlphabetSeat = "ABCDEFGHI";
    private string? _model;
    private int? _productionYear;
    private readonly List<Seat> _seats = [];
    private Guid? _id;

    public PlaneBuilder WithModel(string model)
    {
        _model = model;
        return this;
    }

    public PlaneBuilder WithProductionYear(int productionYear)
    {
        _productionYear = productionYear;
        return this;
    }

    public PlaneBuilder WithSeats(int row,int seatsInRow, SeatClass seats)
    {
        InvalidSeatsInRowException.IfMoreThan(AlphabetSeat.Length, seatsInRow);
        
        if(row == 0 || seatsInRow == 0)
        {
            throw new InvalidSeatsInRowException("Row and seatsInRow cannot be 0");
        }
        
        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < seatsInRow; j++)
            {
                _seats.Add(new Seat(i,$"{i}{AlphabetSeat[j]}", seats));
            }
        }
        
        return this;
    }
    public PlaneBuilder WitId(Guid id)
    {
        _id = id;
        return this;
    }

    public Plane Build()
    {
        if (_model is null)
        {
            throw new InvalidPlaneBuilderProperty("Model");
        }

        if (_productionYear is null)
        {
            throw new InvalidPlaneBuilderProperty("ProductionYear");
        }
        
        _id ??= Guid.NewGuid();

        return Plane.Create(_id, _model, _productionYear.Value, _seats);
    }
}