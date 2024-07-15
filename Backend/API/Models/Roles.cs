using System.ComponentModel.DataAnnotations;

namespace API;

public class Roles
{
    [Key] public int RolesID {get;set;}
    public required string Name {get;set;}
}
