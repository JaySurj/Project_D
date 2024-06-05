﻿using Dropbox.Api;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Project_D
{
    class DropboxAPIToken
    {

        public static string Token = "sl.B2mThBUpIG-mdkLVMrGEQxx3o6wmHxbHLn9WF8m6zXZTuqOVEQSzunjOu57WAbrzUCaLE_vehrnJ-ysWx64NdYJUq_VcTRG0VnC-i_d1rhV31cUyUN9M0tXgjVUNlZAHEJuHmpDaMkefo-M";
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
