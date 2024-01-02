using BrandMonitor.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BrandMonitor.Data;

/// <summary>
/// Контекст БД
/// </summary>
public sealed class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Задачи
    /// </summary>
    public DbSet<TaskInfo> Tasks => Set<TaskInfo>();

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="options">Настройки контекста БД</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
