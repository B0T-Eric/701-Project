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
        //list of events, which can contain empty or full scorecards
        public List<Event> Events { get; set; }

        public string division {  get; set; }
        //new users
        public User(string firstname, string lastname, string club, int nzfaa, int affilitaion, string division, DateOnly DOB) 
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
        public User(string archerName, string clubName, int nZFAANumber, int affiliationNumber, DateOnly dateOfBirth, List<Event> events, string division)
        {
            ArcherName = archerName;
            ClubName = clubName;
            NZFAANumber = nZFAANumber;
            AffiliationNumber = affiliationNumber;
            DateOfBirth = dateOfBirth;
            Events = events;
            this.division = division;
            isGuest = false;
        }
        //guest loading
        public User( List<Event> events)
        {
            ArcherName = "Guest";
            Events = events;
            isGuest = true;
        }
        //guest first login
        public User()
        {
            ArcherName = "Guest";
            Events = new List<Event>();
            isGuest = true;
        }

    }
}
