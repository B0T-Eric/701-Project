using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Club
    {
        [Key] public int ClubID {get;set;}
        public required string Name {get;set;}
        public required string Region {get;set;}
        public int AffiliationID {get;set;}       
        [ForeignKey(nameof(Affiliation))] public Affiliation? Affiliation {get; init;}
        public List<User> Users {get; set;} = new List<User>();
        public List<Event> Events {get; set;} = new List<Event>();
        public List<User_Details> UserDetails {get; set;} = new List<User_Details>();
    }
}