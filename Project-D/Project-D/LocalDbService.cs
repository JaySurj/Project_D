using Dropbox.Api;
using Dropbox.Api.Files;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Project_D
{
    public class LocalDbService
    {
        private readonly SQLiteAsyncConnection _db;
        private readonly DropboxClient _dbx;
        private readonly string _dbPath;

        public LocalDbService()
        {
            _dbPath = DatabaseConfig.DatabasePath;
            _dbx = new DropboxClient("sl.B2Co7f-lbnKfyc-OKac4Ue6ylfvflmfe2i-eafSZdc0MufOKileTw099rj_I48csgBLk9DBRfvRnuzNzGS4mroV5KkwlaDJAPKbJUYIz7F2mJHOY6uzLX6RSdaJTHFyy_T7vCKO65atvjSM");
            _db = new SQLiteAsyncConnection(_dbPath);
            _db.CreateTableAsync<User>().Wait();
        }
        public async Task DownloadDatabaseFromDropbox(string folder, string fileName)
        {
            try
            {
                var response = await _dbx.Files.DownloadAsync(folder + "/" + fileName);
                using (var fileContentStream = await response.GetContentAsStreamAsync())
                {
                    using (var fileStream = new FileStream(_dbPath, FileMode.Create, FileAccess.Write))
                    {
                        await fileContentStream.CopyToAsync(fileStream);
                    }
                }
                Console.WriteLine("Downloaded {0}/{1}", folder, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Download failed: " + ex.Message);
            }
        }

        public async Task UploadDatabaseToDropbox(string folder, string fileName)
        {
            try
            {
                using (var fileStream = new FileStream(_dbPath, FileMode.Open, FileAccess.Read))
                {
                    await _dbx.Files.UploadAsync(
                        folder + "/" + fileName,
                        WriteMode.Overwrite.Instance,
                        body: fileStream);
                }
                Console.WriteLine("Saved {0}/{1}", folder, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Upload failed: " + ex.Message);
            }
        }

        public Task<List<User>> GetUsers()
        {
            return _db.Table<User>().ToListAsync();
        }

        public Task<User> GetByID(int id)
        {
            return _db.Table<User>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<User> GetByEmail(string email)
        {
            return _db.Table<User>().Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task Create(User user)
        {
            await DownloadDatabaseFromDropbox("/Project_D", DatabaseConfig.DatabaseName);
            await _db.InsertAsync(user);
            await UploadDatabaseToDropbox("/Project_D", DatabaseConfig.DatabaseName);
        }

        public async Task Update(User user)
        {
            await DownloadDatabaseFromDropbox("/Project_D", DatabaseConfig.DatabaseName);
            await _db.UpdateAsync(user);
            await UploadDatabaseToDropbox("/Project_D", DatabaseConfig.DatabaseName);
        }
    }
    public static class DatabaseConfig
    {
        public const string DatabaseName = "app.db";
        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseName);
    }

}
