using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ExampleWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SwiftMessagesController : ControllerBase
    {
        private readonly ILogger<SwiftMessagesController> _logger;

        public SwiftMessagesController(ILogger<SwiftMessagesController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFileAsync(IFormFile fileInput)
        {
            if (fileInput == null || fileInput.Length == 0)
            {
                return BadRequest();
            }

            var blaFileName = fileInput.FileName;
            var blaName = fileInput.Name;
            var blaContentType = fileInput.ContentType;
            var blaLength = fileInput.Length;
            var blaHeaders = fileInput.Headers;

            using (var reader = new StreamReader(fileInput.OpenReadStream()))
            {
                var contentString = await reader.ReadToEndAsync();

                var swiftSegmentsMatch = Regex.Matches(contentString, "{\\d+:*}");//contentString.Split('{', StringSplitOptions.RemoveEmptyEntries);
                var basicHeaderBlock = Regex.Split(contentString, "{1:*}");
                var applicationHeaderBlock = Regex.Split(contentString, "{2:*}");
                var userHeaderBlock = Regex.Split(contentString, "{3:*}");
                var textHeaderBlock = Regex.Split(contentString, "{4:*}");
                var trailerHeaderBlock = Regex.Split(contentString, "{5:*}");
                var swiftSegments = Regex.Split(contentString, "{\\d+:");//contentString.Split('{', StringSplitOptions.RemoveEmptyEntries);
                /*
                {1:} Basic Header Block
                {2:} Application Header Block
                {3:} User Header Block
                {4:} Text Block
                {5:} Trailer Block
                */
            }

            return Ok();
        }
    }
}