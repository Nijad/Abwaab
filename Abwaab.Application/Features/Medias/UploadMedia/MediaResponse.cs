namespace Abwaab.Application.Features.Medias.UploadMedia
{
    public class MediaResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string FilePath { get; set; } = string.Empty;
    }
}
