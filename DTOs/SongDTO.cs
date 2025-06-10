namespace MBStream.DTOs
{
    // For CREATE/UPDATE
    public class SongDTO
    {
        public string SongTitle { get; set; } = null!;
        public DateTime? ReleaseDate { get; set; } // <-- Make nullable
        public int DurationSeconds { get; set; }
        public int? AlbumId { get; set; }
        public List<int> ArtistIds { get; set; } = new();
    }


    // For RESPONSES
    public class SongResponseDTO : SongDTO
    {
        public int SongId { get; set; }
        public List<ArtistResponseDTO> Artists { get; set; } = new();
    }


}
