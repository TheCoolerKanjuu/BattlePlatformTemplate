using Microsoft.AspNetCore.Mvc;

namespace Presentation.BP.Bases;

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