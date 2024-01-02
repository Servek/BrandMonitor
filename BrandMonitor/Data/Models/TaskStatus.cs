using System.Text.Json.Serialization;

namespace BrandMonitor.Data.Models;

/// <summary>
/// Статусы задач
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<TaskStatus>))]
public enum TaskStatus
{
    /// <summary>
    /// Создана
    /// </summary>
    Created = 1,

    /// <summary>
    /// В обработке
    /// </summary>
    Running = 2,

    /// <summary>
    /// Завершена
    /// </summary>
    Finished = 3,
}
