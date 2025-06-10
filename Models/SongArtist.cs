using MBStream.Models;

namespace MBStream.Models
{
    // Join table for many-to-many between Song and Artist (for features/collabs)
    public class SongArtist
    {
        public int SongId { get; set; }
        public Song Song { get; set; } = null!;

        public int ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;
    }
}
