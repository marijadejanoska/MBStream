namespace MBStream.DTOs
{
    // For creating or updating an artist (client does not send ArtistId)
    public class ArtistDTO
    {
        public string ArtistName { get; set; } = null!;
    }

    // For responses (client receives ArtistId)
    public class ArtistResponseDTO
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; } = null!;
    }
}
