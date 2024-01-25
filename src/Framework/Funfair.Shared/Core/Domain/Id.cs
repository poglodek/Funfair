

using Newtonsoft.Json;

namespace Funfair.Shared.Domain;

[JsonObject]
[JsonConverter(typeof(IdJsonConverter))]
public sealed record Id
{
    [JsonProperty("value")]
    public Guid Value { get; private set; }

    private Id() { }

    [JsonConstructor]
    public Id(Guid value)
    {
        Value = value;
    }
    
    public static implicit operator string(Id obj) => obj.Value.ToString();
    public static implicit operator Id(Guid obj) => new (obj);
    public static implicit operator Guid(Id obj) => obj.Value;
    
    public static Id FromString(string? id) => new (Guid.Parse(id));
}

public class IdJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Id);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var guidValue = Guid.Parse(reader.Value.ToString());
        return new Id(guidValue);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var id = (Id)value;
        writer.WriteValue(id.Value.ToString());
    }
}
