using System.ComponentModel.DataAnnotations;

namespace BrandMonitor.Data.Models;

/// <summary>
/// Задача
/// </summary>
public class TaskInfo
{
    /// <summary>
    /// GUID задачи
    /// </summary>
    [Required]
    public Guid Id { get; private init; }

    /// <summary>
    /// Дата последнего обновления задачи
    /// </summary>
    [Required]
    public DateTime LastUpdated { get; private set; }

    /// <summary>
    /// Статус задачи
    /// </summary>
    [Required]
    public TaskStatus Status { get; private set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    public TaskInfo()
    {
        Id = Guid.NewGuid();
        LastUpdated = DateTime.UtcNow;
        Status = TaskStatus.Created;
    }

    /// <summary>
    /// Установка статуса
    /// </summary>
    /// <param name="status">Целевой статус</param>
    public void SetStatus(TaskStatus status)
    {
        LastUpdated = DateTime.UtcNow;
        Status = status;
    }
}
