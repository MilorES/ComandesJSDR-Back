using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ComandesAPI.Controllers;

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
    public IActionResult HandleError()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context?.Error;

        if (exception != null)
        {
            _logger.LogError(exception, "S'ha produït un error no controlat: {Message}", exception.Message);
        }

        return Problem(
            title: "S'ha produït un error en processar la sol·licitud",
            statusCode: StatusCodes.Status500InternalServerError
        );
    }
}
