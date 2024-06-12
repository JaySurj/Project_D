using Dropbox.Api;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Project_D
{
    class DropboxAPIToken
    {

        public static string Token = "sl.B29Ht6ekr9jSrnFTfkip7BlA9Ci7aHi3XhlLG1XB95s7OWVk_cqsMn6lQXCPtvxmBgqYR4bqTGrOqeDLO0XiHEBYx6OtmYuS645rzYLoR2pOcEtvJSAYk5m9mwY9BvrvnVhXbCZ3qMX3VRA";
        private readonly DropboxClient _dbx = new DropboxClient(Token);

        public async Task DownloadFileFromDropbox(string dropboxFolder, string dropboxFileName, string localFilename)
        {
            try
            {
                var response = await _dbx.Files.DownloadAsync(dropboxFolder + "/" + dropboxFileName);
                using (var fileContentStream = await response.GetContentAsStreamAsync())
                {
                    var localFilePath = Path.Combine(FileSystem.AppDataDirectory, localFilename);
                    using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await fileContentStream.CopyToAsync(fileStream);
                    }
                }
                Debug.WriteLine($"Downloaded {dropboxFolder}/{dropboxFileName} to {localFilename}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Download failed: {ex.Message}");
            }
        }

    }
}
