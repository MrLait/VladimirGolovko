using System;
using System.Drawing;
using System.IO;
using System.Web;

namespace ClassicMvc.Infrastructure.Utils
{
    public static class FileUtil
    {
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