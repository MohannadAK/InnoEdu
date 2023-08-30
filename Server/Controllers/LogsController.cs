namespace Logger.Server;

[Route("api/[controller]")]
[ApiController]
public class LogsController : ControllerBase
{
    [HttpPost("{route}")]
    public async Task<IActionResult> PostLog(string route, [FromBody] string content)
    {
        switch (route.ToLower())
        {
            case "information":
                ArgumentNullException.ThrowIfNull(content);
                await Log.WriteInformation(content);
                break;

            case "debug":
                ArgumentNullException.ThrowIfNull(content);
                await Log.WriteDebug(content);
                break;

            case "error":
                ArgumentNullException.ThrowIfNull(content);
                await Log.WriteError(content);
                break;

            case "critical":
                ArgumentNullException.ThrowIfNull(content);
                await Log.WriteCritical(content);
                break;

            default:
                return BadRequest("Invalid route.");
        }

        return Ok("Log entry added.");
    }

    [HttpGet("{route}")]
    public async Task<IActionResult> GetLogs(string route)
    {
        string content;
        switch (route.ToLower())
        {
            case "alllogs":
                content = await Log.ReadAllLogs();
                break;

            case "information":
                content = await Log.ReadInformationLogs();
                break;

            case "debug":
                content = await Log.ReadDebugLogs();
                break;

            case "error":
                content = await Log.ReadErrorLogs();
                break;

            case "critical":
                content = await Log.ReadCriticalLogs();
                break;

            default:
                return BadRequest("Invalid route.");
        }

        ArgumentNullException.ThrowIfNull(content);

        return Ok(content);
    }
}