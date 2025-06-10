using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MBStream.Models;

namespace MBStream.Models
{
    // Represents a song/track
    public class Song
    {
        [Key]
        public int SongId { get; set; }

        [Required, MaxLength(100)]
        public string SongTitle { get; set; } = null!;

        public DateTime? ReleaseDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Duration must be positive.")]
        public int? DurationSeconds { get; set; } // Song length in seconds

        // Song can belong to one album (optional)
        public int? AlbumId { get; set; }
        public Album? Album { get; set; }

        // Song can have multiple artists (featuring, collabs)
        public List<SongArtist> SongArtists { get; set; } = new();

        // Users can like songs (many-to-many)
        public List<UserSong> LikedByUsers { get; set; } = new();

        // Song can be in many playlists (many-to-many)
        public List<PlaylistSong> PlaylistSongs { get; set; } = new();
    }
}
