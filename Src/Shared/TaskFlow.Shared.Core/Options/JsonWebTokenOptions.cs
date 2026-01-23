namespace TaskFlow.Shared.Core.Options {
    public class JsonWebTokenOptions {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
    }
}
