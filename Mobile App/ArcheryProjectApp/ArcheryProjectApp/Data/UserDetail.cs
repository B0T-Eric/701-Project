using SQLite;

namespace ArcheryProjectApp.Data
{
    [Table("user_detail")]
    public class UserDetail
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("division")]
        public string Division { get; set; }
        [Column("nzfaa_num")]
        public int NzfaaNumber { get; set; }
        [Column("club_number")]
        public int ClubNumber {  get; set; }
        [Column("club_name")]
        public string ClubName { get; set; }
        [Column("DOB")]
        public DateOnly DateOfBirth { get; set; }
        [Column("user_auth_id")]
        public int UserAuthId { get; set;}

        //Users need events (user events)
    }
}
