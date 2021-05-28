using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace TicketManagement.WebMVC.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly string _picsPath = "Pics/";
        private readonly string _wwwrootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\wwwroot\"));

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

        public string SaveImageToFolder(string base64String)
        {
            var image = ConvertBase64ToImage(base64String);
            var imageName = string.Format($"{_picsPath}{image.FrameDimensionsList[0]}.{ImageFormat.Png}");
            image.Save(string.Format($"{_wwwrootPath}{imageName}"));
            return imageName;
        }

        private static Image ConvertBase64ToImage(string base64String)
        {
            byte[] imageBytes = ConvertFromBase64(base64String);

            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        private static byte[] ConvertFromBase64(string base64String)
        {
            byte[] bytes;

            try
            {
                bytes = Convert.FromBase64String(base64String);
            }
            catch (FormatException)
            {
                Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
                base64String = regex.Replace(base64String, string.Empty);
                bytes = Convert.FromBase64String(base64String);
            }

            return bytes;
        }
    }
}
