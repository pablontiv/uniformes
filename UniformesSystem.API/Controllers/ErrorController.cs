using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UniformesSystem.API.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [Route("/error")]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        _logger.LogError(exception, "An unhandled exception has occurred");

        return Problem(
            title: "An error occurred while processing your request.",
            statusCode: StatusCodes.Status500InternalServerError);
    }
}
