using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CulDeSacServicesOrchestration.Core.Models;

public class Student
{
    [Required, JsonPropertyName("id")]
    public Guid Id { get; set; }

    [Required, JsonPropertyName("name")]
    public string Name { get; set; }
}