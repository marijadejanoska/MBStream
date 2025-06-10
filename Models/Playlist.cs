using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MBStream.Models;

namespace MBStream.Models
{
    // Represents a playlist created by a user
    public class Playlist
    {
        [Key]
        public int PlaylistId { get; set; }

        [Required, MaxLength(100)]
        public string PlaylistTitle { get; set; } = null!;

        // The user who created the playlist
        public int UserId { get; set; }
        public User? User { get; set; }

        // Playlist contains many songs (many-to-many)
        public List<PlaylistSong> PlaylistSongs { get; set; } = new();
    }
}
