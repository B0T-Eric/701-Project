using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class User_Details
    {
        [Key] public int DetailID {get;set;}
        public string Archers_Name {get;set;} = default!;
        public bool Fee_Paid {get;set;}
        [DataType(DataType.Date)] public DateTime Fee_Paid_Date {get;set;}
        public int UserID {get;set;}       
        [ForeignKey(nameof(UserID))] public User? Users {get; init;}
        public int DivisionsID {get;set;}       
        [ForeignKey(nameof(DivisionsID))] public Divisions? Divisions {get; init;}
        public int ClubID {get;set;}       
        [ForeignKey(nameof(ClubID))] public Club? Clubs {get; init;}
        public int ScoreID {get;set;}       
        [ForeignKey(nameof(ScoreID))] public Score? Scores {get; init;}
    }
}