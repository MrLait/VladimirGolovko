using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace ClassicMvc.Infrastructure.Utils
{
    public static class FileUtil
    {
        public static async Task<string> UploadInBase64Format(string imageName)
        {
            var path = Path.Combine(HostingEnvironment.MapPath("~/App_Data/"), imageName);
            using (var memoryStream = new MemoryStream())
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
                return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public static void Download(HttpPostedFileBase file)
        {
            var path = Path.Combine(HostingEnvironment.MapPath("~/App_Data/"), file.FileName);

            var data = new byte[file.ContentLength];
            file.InputStream.Read(data, 0, file.ContentLength);

            using (var sw = new FileStream(path, FileMode.Create))
            {
                sw.Write(data, 0, data.Length);
            }
        }

        public static string ConvertImageToBase64(HttpPostedFileBase file)
        {
            using (BinaryReader theReader = new BinaryReader(file.InputStream))
            {
                var thePictureAsBytes = theReader.ReadBytes(file.ContentLength);
                string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
                return string.Format($"data:image/png;base64,{thePictureDataAsString}");
            }
        }

        public static Image ConvertBase64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }
    }
}