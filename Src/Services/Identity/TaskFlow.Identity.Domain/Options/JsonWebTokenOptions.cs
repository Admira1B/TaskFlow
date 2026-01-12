namespace TaskFlow.Identity.Domain.Options {
    public class JsonWebTokenOptions {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public string CookieName { get; set; } = null!;
        public int ExpiresHours { get; set; }
    }
}
