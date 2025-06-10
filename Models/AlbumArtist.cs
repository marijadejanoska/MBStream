using MBStream.Models;

namespace MBStream.Models
{
    // Join table for many-to-many between Album and Artist (for collaborative albums)
    public class AlbumArtist
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;

        public int ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;
    }
}
