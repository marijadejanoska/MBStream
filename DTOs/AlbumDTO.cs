namespace MBStream.DTOs
{
    public class AlbumDTO
    {
        public string AlbumTitle { get; set; } = null!;
        public DateTime? ReleaseDate { get; set; }
        public List<int> ArtistIds { get; set; } = new(); // IDs of artists for this album
    }

    public class AlbumResponseDTO : AlbumDTO
    {
        public int AlbumId { get; set; }
        public List<ArtistResponseDTO> Artists { get; set; } = new(); // <-- Use ArtistResponseDTO here
        public List<SongDTO> Songs { get; set; } = new();
    }

}
