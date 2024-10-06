using Android.Icu.Text;
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
            _connection.CreateTableAsync<UserDetail>();
            _connection.CreateTableAsync<UserEvents>();
            _connection.CreateTableAsync<RoundTable>();
            _connection.CreateTableAsync<EndTable>();
            _connection.CreateTableAsync<ScoreItem>();
            _connection.CreateTableAsync<RegisterdEvents>();
        }
        public async Task<UserAuth> GetUserById(int id)
        {
            return await _connection.Table<UserAuth>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task Create(UserAuth auth)
        {
            await _connection.InsertAsync(auth);
        }



        //Add scores from ends in app
        public async Task AddScoresToDatabase(int endId, List<string> scores)
        {
            foreach (var score in scores)
            {
                var scoreItem = new ScoreItem { EndId = endId, Score = score };
                await _connection.InsertAsync(scoreItem);
            }
        }
        //retrieve scores from and end
        public async Task<List<string>> GetScoresFromDatabase(int endId)
        {
            var scoreItems = await _connection.Table<ScoreItem>().Where(item => item.EndId == endId).ToListAsync();
            return scoreItems.Select(item => item.Score).ToList();
        }
    }
}
