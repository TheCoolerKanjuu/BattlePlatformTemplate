using Microsoft.AspNetCore.Mvc;

namespace Presentation.BF.Bases;

[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    protected readonly ILogger<BaseController> Logger;

    protected BaseController(ILogger<BaseController> logger)
    {
        this.Logger = logger;
    }
}