using System.Text;

namespace Abwaab.Infrastructure.Identity
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; } // in minutes

        // Helper method to get the secret key as bytes (used for signing)
        public byte[] GetSecretBytes() => Encoding.UTF8.GetBytes(Secret);

        // Optional: Validate that required fields are not empty
        public bool IsValid() =>
            !string.IsNullOrWhiteSpace(Secret) &&
            !string.IsNullOrWhiteSpace(Issuer) &&
            !string.IsNullOrWhiteSpace(Audience) &&
            ExpiryMinutes > 0;
    }
}
