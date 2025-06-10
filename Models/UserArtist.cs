using MBStream.Models;

namespace MBStream.Models
{ 
    // Join table for many-to-many between User and Artist (user follows artist)
    public class UserArtist
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;
    }
}
