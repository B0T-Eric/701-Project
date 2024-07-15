using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Affiliation
    {
        [Key] public int AffiliationID {get;set;}
        public required string Name {get;set;}
        public List<Club> Clubs {get;set;} = new List<Club>();
    }
}