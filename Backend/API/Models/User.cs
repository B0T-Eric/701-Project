using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class User
    {
       [Key] public int UserID {get;set;}
       public string Affiliation_Number {get; set;} = default!;
       public required string FirstName {get;set;}
       public required string SurName {get;set;}
       [DataType(DataType.Date),Required] public DateTime DOB {get;set;}
       [EmailAddress] public required string Email { get; set;}
       [Phone] public string? Phone { get; set; }
       public required string Emergency_Contact {get;set;}
       public int ClubID {get;set;}       
       [ForeignKey(nameof(Club))] public Club? Club {get; init;}
       public List<Divisions> Divisions {get; set;} = [];
       public List<User_Details> UserDetails {get; set;} = [];
       public List<Score> Scores {get; set;} = new List<Score>();
    }
}