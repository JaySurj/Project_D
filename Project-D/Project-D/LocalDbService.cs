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

        public LocalDbService()
        {
            var xamlDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _dbPath = Path.Combine(xamlDirectory, DB_NAME);

            _db = new SQLiteAsyncConnection(_dbPath);
            _db.CreateTableAsync<User>().Wait();
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
            return _db.InsertAsync(user);
        }

        public Task Update(User user)
        {
            return _db.UpdateAsync(user);
        }

    }
}
