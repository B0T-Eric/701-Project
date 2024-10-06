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
        [Column("arrow_count")]
        public int ArrowCount { get; set; }
        [Column("distance")]
        public string? Distance { get; set; }
        [Column("target_name")]
        public string? TargetName { get; set; }

        public List<EndTable> Ends { get; set; }
    }
}
