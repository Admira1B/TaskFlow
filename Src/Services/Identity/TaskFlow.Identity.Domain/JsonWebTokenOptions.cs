namespace TaskFlow.Identity.Domain {
    public class JsonWebTokenOptions {
        public string Issuer { get; set; } = null!;
        public List<string> Audiences { get; set; } = [];
        public string SecretKey { get; set; } = null!;
        public int ExpiresHours { get; set; }
    }
}
