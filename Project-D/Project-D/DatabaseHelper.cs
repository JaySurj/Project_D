using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project_D
{
    public static class DatabaseHelper
    {
        public static async Task<List<User>> GetUsersAsync(string dbPath)
        {
            //getuserAsync
            var db = new SQLiteAsyncConnection(dbPath);
            return await db.Table<User>().ToListAsync();
        }
    }
}