namespace Planes.Core.ValueObjects;

public record Model(string Value)
{
    public static implicit operator string(Model model)
        => model.Value;

    public static implicit operator Model(string value)
        => new(value);
}