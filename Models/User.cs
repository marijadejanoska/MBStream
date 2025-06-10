using MBStream.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MBStream.Models
{
    // Represents a user of the platform
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string UserName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string UserEmail { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "User"; // Default to "User", can be "Admin"

        // User can create many playlists
        public List<Playlist> Playlists { get; set; } = new();

        // User can follow many artists (many-to-many)
        public List<UserArtist> FollowedArtists { get; set; } = new();

        // User can like many songs (many-to-many)
        public List<UserSong> LikedSongs { get; set; } = new();
    }

}
