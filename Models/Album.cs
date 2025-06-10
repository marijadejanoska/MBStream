using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MBStream.Models;

namespace MBStream.Models
{
    // Represents an album (can be by one or more artists)
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }

        [Required, MaxLength(100)]
        public string AlbumTitle { get; set; } = null!;

        public DateTime? ReleaseDate { get; set; }

        // Album can have multiple artists (collaborative albums)
        public List<AlbumArtist> AlbumArtists { get; set; } = new();

        // Album contains many songs (one-to-many)
        public List<Song> Songs { get; set; } = new();
    }
}


