namespace MBStream.DTOs
{
    public class PlaylistDTO
    {
        public string PlaylistTitle { get; set; } = null!;
        public int UserId { get; set; }
        public List<int> SongIds { get; set; } = new();
    }

    public class PlaylistResponseDTO : PlaylistDTO
    {
        public int PlaylistId { get; set; }
        public string PlaylistTitle { get; set; } = null!;
        public int UserId { get; set; }
        public List<SongResponseDTO> Songs { get; set; } = new(); // Instead of SongIds
    }
}
