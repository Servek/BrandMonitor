using BrandMonitor.Data;
using BrandMonitor.Data.Models;
using BrandMonitor.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrandMonitor.Controllers;

/// <summary>
/// Контроллер задач
/// </summary>
[ApiController]
[Route("task")]
public class TaskController : ControllerBase
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="context">Контекст БД</param>
    public TaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получение информации о задаче
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информация о задаче</returns>
    /// <response code="200">Запрос выполнен успешно</response>
    /// <response code="400">Некорректно указаны параметры запроса</response>
    /// <response code="404">Задача с указанным идентификатором не найдена</response>
    /// <response code="500">Что-то пошло не так</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, new ValidationProblemDetails(ModelState));

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (task == default)
            return StatusCode(StatusCodes.Status404NotFound, "Задача с указанным идентификатором не найдена");

        return StatusCode(StatusCodes.Status200OK, task);
    }

    /// <summary>
    /// Создание задачи
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информация о созданной задаче</returns>
    /// <response code="202">Задача принята к обработке</response>
    /// <response code="500">Что-то пошло не так</response>
    [HttpPost]
    [ProducesResponseType(typeof(TaskInfo), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(CancellationToken cancellationToken)
    {
        var task = new TaskInfo();
        _context.Add(task);
        await _context.SaveChangesAsync(cancellationToken);

        BackgroundJob.Enqueue<TaskHandlingService>(x => x.HandleTask(task.Id, CancellationToken.None));

        return StatusCode(StatusCodes.Status202Accepted, task);
    }
}
