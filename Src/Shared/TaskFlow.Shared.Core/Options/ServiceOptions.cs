using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace TaskFlow.Shared.Core.Options {
    public class ServiceOptions {
        [Required(ErrorMessage = "Service name is required")]
        public required string Name { get; init; }

        [Required(ErrorMessage = "Service host is required")]
        public required string Host { get; init; }

        [Required(ErrorMessage = "Service port is required")]
        [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
        public int Port { get; init; }

        [JsonIgnore]
        public string Address => $"http://{Host}:{Port}";
    }

    [OptionsValidator]
    public partial class ServiceOptionsValidator : IValidateOptions<ServiceOptions>{
    }
}
