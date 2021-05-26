using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace TicketManagement.WebMVC.Services.FileServices
{
    public class FileService : IFileService
    {
        public string ConvertFileToString(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    result.AppendLine(reader.ReadLine());
                }
            }

            return result.ToString();
        }
    }
}
