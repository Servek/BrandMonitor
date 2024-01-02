using BrandMonitor.Data;
using Hangfire;
using TaskStatus = BrandMonitor.Data.Models.TaskStatus;

namespace BrandMonitor.Services;

/// <summary>
/// Сервис обработки задач
/// </summary>
public class TaskHandlingService
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="context">Контекст БД</param>
    public TaskHandlingService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Обработка задачи
    /// </summary>
    /// <param name="id">GUID задачи</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public async Task HandleTask(Guid id, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FindAsync([id], cancellationToken: cancellationToken);
        if (task == default)
            throw new Exception("Задача не найдена");

        task.SetStatus(TaskStatus.Running);
        await _context.SaveChangesAsync(cancellationToken);

        await Task.Delay(TimeSpan.FromMinutes(2), cancellationToken);

        task.SetStatus(TaskStatus.Finished);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
