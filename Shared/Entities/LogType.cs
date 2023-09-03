using System.Text.Json.Serialization;

namespace InnoEdu.Shared.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LogType
{
    Information,
    Debug,
    Error,
    Critical
}