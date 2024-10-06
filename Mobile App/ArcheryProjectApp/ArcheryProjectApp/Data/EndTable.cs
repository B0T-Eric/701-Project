using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryProjectApp.Data
{
    [Table("end_table")]
    public class EndTable
    {
        [PrimaryKey,AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("target_name")]
        public string? TargetName { get; set; }
        [Column("distance")]
        public string? Distance { get; set; }
        [Column("position")]
        public string Position { get; set; } = default!;


    }
    [Table("score_item")]
    public class ScoreItem
    {
        [PrimaryKey, AutoIncrement][Column("id")]
        public int Id { get; set; }
        [Column("end_id")]
        public int EndId { get; set; }
        [Column("score")]
        public string Score {  get; set; }
    }
}
