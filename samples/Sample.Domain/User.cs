using System.ComponentModel.DataAnnotations;
namespace Sample.Domain;
public class User
{
    public string Id { get; set; }

    [MaxLength(255)]
    public string FirstName { get; set; }

    [MaxLength(255)]
    public string LastName { get; set; }

    [MaxLength(255)]
    public string SubscriptionLevel { get; set; }

    [MaxLength(2)]
    public string Country { get; set; }
    public Role Roles { get; set; }
    public bool IsInRole(Role role) => Roles.HasFlag(role);
    public User()
    {
    }
    public User(string id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Roles = Role.None;
    }
}