using MediatR;
using Planes.App.Commands.Dto;

namespace Planes.App.Commands;

public sealed record CreatePlaneCommand(string Model, int ProductionYear, List<Seats>? SeatsList) : IRequest<PlaneIdDto>;

public sealed record Seats(int Rows, int NumberInRow, string SeatClass);