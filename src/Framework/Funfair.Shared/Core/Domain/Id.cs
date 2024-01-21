using Newtonsoft.Json;

namespace Funfair.Shared.Domain;

public sealed record Id
{
    [JsonProperty("value")]
    public Guid Value { get; private set; }

    private Id() { }

    public Id(Guid value)
    {
        Value = value;
    }
    
    public static implicit operator string(Id obj) => obj.Value.ToString();
    public static implicit operator Id(Guid obj) => new (obj);
    public static implicit operator Guid(Id obj) => obj.Value;
}