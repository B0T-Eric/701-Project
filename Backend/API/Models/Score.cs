using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Score
    {
        [Key] public int ScoreID {get;set;}
        public string Target_Types {get;set;} = default!;
        public int Scores {get;set;}
        [DataType(DataType.Date)] public DateTime Date {get;set;}
        public int Ends {get;set;}
        public int Round {get;set;}
        public int UserID {get;set;}       
        [ForeignKey(nameof(User))] public User? Users {get; init;}
        public int EventID {get;set;}       
        [ForeignKey(nameof(Event))] public Event? Events {get; set;}
        public List<User_Details> UserDetails {get; set;} = [];
    }
}