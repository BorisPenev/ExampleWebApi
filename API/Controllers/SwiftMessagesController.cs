using Microsoft.AspNetCore.Mvc;
using Application.SwiftMessages;

namespace API.Controllers;

public class SwiftMessagesController : BaseApiController
{
    private readonly ILogger<SwiftMessagesController> _logger;

    public SwiftMessagesController(ILogger<SwiftMessagesController> logger)
    {
        _logger = logger;
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file)
    {
        return HandleResult(await Mediator.Send(new UploadFile.Command { File = file }));
    }
}