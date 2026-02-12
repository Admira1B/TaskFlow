using System.Text.Json.Serialization;

namespace TaskFlow.Identity.Application.Options {
    public class JsonWebTokenGenerationOptions {
        public string Issuer { get; set; } = null!;
        public string[] ValidAudiences { get; set; } = [];
        public string SecretKey { get; set; } = null!;
        public string ExpiresHours { get; set; } = null!;

        [JsonIgnore]
        public int ExpiresHoursParsed => int.Parse(ExpiresHours);
    }
}
