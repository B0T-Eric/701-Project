using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Divisions
    {
        [Key] public int DivisionsID {get;set;}
        public required string Age {get;set;}
        public required string Age_Type {get;set;}
        public required string Shooting {get;set;}
        public required string Shooting_Style {get;set;}
        public required string Division {get;set;}
        public required string Division_Style {get;set;}
        public int UserID {get;set;}       
        [ForeignKey(nameof(User))] public User? User {get; init;}
        public List<User_Details> UserDetails {get; set;} = [];
    }
}