using Newtonsoft.Json;

namespace Users.Core.Entities;

public class Role
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("claims")]
    public IDictionary<string,string> Claims { get; set; } = new Dictionary<string, string>();
    

    public Role()
    {
        Name = Default;
    }
    
    public Role(string name)
    {
        Name = name;
    }
    
    public static string Default => User;
    
    public const string User = nameof(User);
    public const string Worker = nameof(Worker);
    public const string Admin = nameof(Admin);
}