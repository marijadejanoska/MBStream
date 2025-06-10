using MBStream.Models;
using System.ComponentModel.DataAnnotations;

namespace MBStream.Models
{
    // Represents an artist (can be a solo artist or a band)
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }

        [Required, MaxLength(50)]
        public string ArtistName { get; set; } = null!;

        // Artist can have many songs (through SongArtists)
        public List<SongArtist> SongArtists { get; set; } = new();

        // Artist can have many albums (through AlbumArtists)
        public List<AlbumArtist> AlbumArtists { get; set; } = new();

        // Users can follow artists (many-to-many)
        public List<UserArtist> Followers { get; set; } = new();
    }

}


