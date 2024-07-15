using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Event
    {
        [Key] public int EventID {get;set;}        
        public required string Event_Name {get;set;}
        public int ClubID {get;set;}       
        [ForeignKey(nameof(Club))] public Club? Clubs {get; init;}
        public List<Score> Scores {get; set;} = new List<Score>();
    }
}