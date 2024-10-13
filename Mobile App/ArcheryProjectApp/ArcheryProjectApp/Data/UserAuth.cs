using SQLite;

namespace ArcheryProjectApp.Data
{
    [Table("user_auth")]
    public class UserAuth
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Unique]
        [Column("username")]
        public string Username { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("salt")]
        public string? Salt {  get; set; }

    }
}
