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
        private const string DB_NAME = "local_db.db";
        private readonly SQLiteAsyncConnection _db;
        private readonly string _dbPath;
        private readonly DropboxClient _dbx;

        public LocalDbService()
        {
            _dbx = new DropboxClient("sl.B132g8LuSE0DAUpQjfxH38aJXVpsAAaPsMahcLQW3j37zFQEMhXdqOQ7VCnSTbfcDs8K6TFYGUBXn1S33dstuUzLKMTb_fLTQ8lHvVFI5_rZpeyxmIN3h-mPJ7p8kFoE5L_9QUhkGE0cPrY");
            var xamlDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _dbPath = Path.Combine(xamlDirectory, DB_NAME);

            _db = new SQLiteAsyncConnection(_dbPath);
            _db.CreateTableAsync<User>().Wait();
            Task.Run(() => UploadDatabaseToDropbox("/Project_D", "local_db.db"));
        }

        public async Task UploadDatabaseToDropbox(string folder, string fileName)
        {
            try
            {
                using (var fileStream = new FileStream(_dbPath, FileMode.Open, FileAccess.Read))
                {
                    var updated = await _dbx.Files.UploadAsync(
                        folder + "/" + fileName,
                        WriteMode.Overwrite.Instance,
                        body: fileStream);

                    Console.WriteLine("Saved {0}/{1} rev {2}", folder, fileName, updated.Rev);
                }
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

        public Task Create(User user)
        {
            //update the database in dropbox
            Task.Run(() => UploadDatabaseToDropbox("/Project_D", "local_db.db"));
            return _db.InsertAsync(user);
        }

        public Task Update(User user)
        {
            //update the database in dropbox
            Task.Run(() => UploadDatabaseToDropbox("/Project_D", "local_db.db"));
            return _db.UpdateAsync(user);
        }
    }

}