using Microsoft.AspNetCore.Http;

namespace Application.UnitTests
{
    public class TestHelpers
    {
        public static IFormFile GenFormFile(string filePath, string mimeType, string name, string fileName)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            IFormFile formFile = new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, name, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = mimeType
            };

            return formFile;
        }
    }
}
