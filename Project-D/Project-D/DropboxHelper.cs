using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Project_D
{
    public static class DropboxHelper
    {
        public static async Task<string> DownloadDatabaseAsync(string dropboxFilePath, string localFileName)
        {
            try
            {
                using (var dbx = new DropboxClient(DropboxAPIToken.Token))
                {
                    var response = await dbx.Files.DownloadAsync(dropboxFilePath);
                    var fileContent = await response.GetContentAsByteArrayAsync();
                    var localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), localFileName);
                    File.WriteAllBytes(localPath, fileContent);
                    Console.WriteLine($"Database downloaded to: {localPath}");
                    return localPath;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
                throw;
            }
        }

        public static async Task UploadDatabaseAsync(string dropboxFilePath, string localFilePath)
        {
            try
            {
                using (var dbx = new DropboxClient(DropboxAPIToken.Token))
                {
                    using (var fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var memStream = new MemoryStream();
                        await fileStream.CopyToAsync(memStream);
                        memStream.Position = 0;

                        await dbx.Files.UploadAsync(
                            dropboxFilePath,
                            WriteMode.Overwrite.Instance,
                            body: memStream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                throw;
            }
        }
    }
}
