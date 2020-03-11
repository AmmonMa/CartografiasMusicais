using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CartografiasMusicais.CrossCutting.Utils
{
    public static class FileService
    {
        public static async Task<string> UploadFileAsync(IFormFile File, string path, string file)
        {
            try
            {
                if (File == null)
                {
                    throw new ArgumentNullException(nameof(File));
                }
                using (var stream = new MemoryStream())
                {
                    await File.CopyToAsync(stream);
                    byte[] fileBytes = stream.ToArray();
                    FileStream fileStream = new FileStream((path + file), FileMode.Create, FileAccess.Write);
                    fileStream.Write(fileBytes, 0, fileBytes.Length);
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }

            return (file);
        }
    }
}
