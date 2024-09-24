using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryProjectApp.Data
{
    public class LocalDbService
    {
        private const string DB_NAME = "Archery_Database";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.ExecuteAsync("PRAGMA foreign_keys = ON");
            _connection.CreateTableAsync<UserAuth>();
        }
        public async Task<UserAuth> GetUserById(int id)
        {
            return await _connection.Table<UserAuth>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task Create(UserAuth auth)
        {
            await _connection.InsertAsync(auth);
        }
    }
}
