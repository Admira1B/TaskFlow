namespace TaskFlow.Identity.Domain.Options {
    public class JsonWebTokenGenerationOptions {
        public string Issuer { get; set; } = null!;
        public string[] ValidAudiences { get; set; } = [];
        public string SecretKey { get; set; } = null!;
        public int ExpiresHours { get; set; }
    }
}
