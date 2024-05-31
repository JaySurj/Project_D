
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project_D
{
    public static class DatabaseHelper
    {
        //database helper
        public static async Task<List<User>> GetUsersAsync(string dbPath)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            return await db.Table<User>().ToListAsync();
        }
    }
}