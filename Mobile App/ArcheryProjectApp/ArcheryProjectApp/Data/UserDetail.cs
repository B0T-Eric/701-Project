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
        public string? FirstName { get; set; } = default!;
        [Column("last_name")]
        public string? LastName { get; set; } = default!;
        [Column("division")]
        public string? Division { get; set; } = default!;
        [Column("nzfaa_num")]
        public int? NzfaaNumber { get; set; }
        [Column("club_number")]
        public int? ClubNumber {  get; set; }
        [Column("club_name")]
        public string ClubName { get; set; } = default!;
        [Column("DOB")]
        public DateOnly? DateOfBirth { get; set; }
        [Column("user_auth_id")]
        public int UserAuthId { get; set;}

        [Ignore]
        public UserAuth? UserAuth { get; set; }
        [Ignore]
        public List<UserEvents>? UserEvents { get; set; }
    }
}
