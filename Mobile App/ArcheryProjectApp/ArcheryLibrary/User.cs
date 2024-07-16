using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    internal class User
    {
        public string ArcherName { get; set; }
        public string ClubName { get; set; }
        public int NZFAANumber { get; set; }
        public int AffiliationNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public List<Round> Rounds { get; set; }
        public Division division {  get; set; }

        public List<Target> targets { get; set; }

        public User() { }
    }
}
