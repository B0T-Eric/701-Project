using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public class User
    {
        public bool isGuest { get; set; }
        public string ArcherName { get; set; }
        
        public string ClubName { get; set; }
        
        public int NZFAANumber { get; set; }
        
        public int AffiliationNumber { get; set; }
        
        public DateOnly DateOfBirth { get; set; }
        
        public List<ScoreCard> ScoreCards { get; set; }

        public Division division {  get; set; }
        public List<Event> RegisteredEvents { get; set; }
        //new users
        public User(string firstname, string lastname, string club, int nzfaa, int affilitaion, Division division, DateOnly DOB) 
        {
            ArcherName = firstname + " " + lastname;
            ClubName = club;
            NZFAANumber = nzfaa;
            AffiliationNumber = affilitaion;
            this.division = division;
            DateOfBirth = DOB;
            isGuest = false;
        }
        //existing user loading
        public User(string archerName, string clubName, int nZFAANumber, int affiliationNumber, DateOnly dateOfBirth, List<ScoreCard> scoreCards, Division division)
        {
            ArcherName = archerName;
            ClubName = clubName;
            NZFAANumber = nZFAANumber;
            AffiliationNumber = affiliationNumber;
            DateOfBirth = dateOfBirth;
            ScoreCards = scoreCards;
            this.division = division;
            isGuest = false;
        }
        //guest loading
        public User( List<ScoreCard> scoreCards)
        {
            ArcherName = "Guest";
            ScoreCards = scoreCards;
            isGuest = true;
        }
        //guest first login
        public User()
        {
            ArcherName = "Guest";
            isGuest = true;
        }

    }
}
