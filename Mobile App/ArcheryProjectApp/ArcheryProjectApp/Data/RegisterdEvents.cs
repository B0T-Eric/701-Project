using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryProjectApp.Data
{
    //This table is for event data for the events which are retrieved from the api and displayed in the upcomming events page.
    [Table("registered_events")]
    public class RegisterdEvents
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("date")]
        public DateOnly Date {  get; set; }
        //this is the scoreCard class details minus the rounds scores

        [Column("round_count")]
        public int RoundCount { get; set; }
        [Column("environment")]
        public string Environment { get; set; }
        [Column("weather")]
        public string? Weather { get; set; }
        [Column("division")]
        public string Division { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Ignore]
        public UserDetail UserDetail { get; set; }
    }
}
