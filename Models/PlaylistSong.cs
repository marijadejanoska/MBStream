using MBStream.Models;

namespace MBStream.Models
{
    // Join table for many-to-many between Playlist and Song
    public class PlaylistSong
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = null!;

        public int SongId { get; set; }
        public Song Song { get; set; } = null!;
    }
}
