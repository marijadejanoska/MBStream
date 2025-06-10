using Microsoft.EntityFrameworkCore;
using MBStream.Models;
using MBStream.Models;

namespace MBStream.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet properties represent your database tables
        public DbSet<User> Users { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }

        // Join tables for many-to-many relationships
        public DbSet<SongArtist> SongArtists { get; set; }
        public DbSet<AlbumArtist> AlbumArtists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<UserArtist> UserArtists { get; set; }
        public DbSet<UserSong> UserSongs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite primary key for SongArtist (SongId + ArtistId)
            modelBuilder.Entity<SongArtist>()
                .HasKey(sa => new { sa.SongId, sa.ArtistId });

            // Composite primary key for AlbumArtist (AlbumId + ArtistId)
            modelBuilder.Entity<AlbumArtist>()
                .HasKey(aa => new { aa.AlbumId, aa.ArtistId });

            // Composite primary key for PlaylistSong (PlaylistId + SongId)
            modelBuilder.Entity<PlaylistSong>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });

            // Composite primary key for UserArtist (UserId + ArtistId)
            modelBuilder.Entity<UserArtist>()
                .HasKey(ua => new { ua.UserId, ua.ArtistId });

            // Composite primary key for UserSong (UserId + SongId)
            modelBuilder.Entity<UserSong>()
                .HasKey(us => new { us.UserId, us.SongId });

            // Index for unique UserEmail
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserEmail)
                .IsUnique();

            // Index for SongTitle (for faster search)
            modelBuilder.Entity<Song>()
                .HasIndex(s => s.SongTitle);

            // Cascade delete playlists when user is deleted
            modelBuilder.Entity<Playlist>()
                .HasOne(p => p.User)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // If album is deleted, keep the song (set AlbumId to null)
            modelBuilder.Entity<Song>()
                .HasOne(s => s.Album)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Song>(entity =>
            {
                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("timestamp without time zone");
            });

            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("timestamp without time zone"); // Match database type
            });


        }
    }
}
