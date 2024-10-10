using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryProjectApp.Data
{
    [Table("round_table")]
    public class RoundTable
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("type")]
        public string Type { get; set; } = default!;
        [Column("end_count")]
        public int EndCount {  get; set; }
        [Column("distance")]
        public string? Distance { get; set; }
        [Column("target_name")]
        public string? TargetName { get; set; }
        [Column("event_id")]
        public int EventId {  get; set; }
        [Ignore]
        public List<EndTable> Ends { get; set; }
    }
}
