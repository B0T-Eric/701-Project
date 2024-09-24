using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ArcheryProjectApp.Data
{
    //table combines event data and score card information from the in app creation process,
    //this will then have an attachment of rounds
    [Table("user_events")]
    internal class UserEvents
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("date")]
        public DateOnly Date {  get; set; }
        [Column("round_count")]
        public int RoundCount { get; set; }
        [Column("environment")]
        public string Environment { get; set; }
        [Column("weather")]
        public string Weather { get; set; }
        [Column("division")]
        public string Division { get; set; }

        public List<RoundTable> Rounds { get; set; }

    }
}
