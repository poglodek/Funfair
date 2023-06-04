namespace Users.Core.Entities;

public class Role
{
    public string Name { get; set; } = string.Empty;
    public IEnumerable<string> Claims { get; set; } = new List<string>();
    public IEnumerable<User> Users { get; set; } = new List<User>();

    public Role()
    {
        Name = Default;
    }
    
    public static string Default => User;
    
    public const string User = nameof(User);
    public const string Worker = nameof(Worker);
    public const string Admin = nameof(Admin);
}