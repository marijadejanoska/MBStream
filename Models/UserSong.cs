using MBStream.Models;

namespace MBStream.Models
{
    // Join table for many-to-many between User and Song (user likes song)
    public class UserSong
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int SongId { get; set; }
        public Song Song { get; set; } = null!;
    }
}
