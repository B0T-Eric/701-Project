using SQLite;
using ArcheryLibrary;
using System.Text;

namespace ArcheryProjectApp.Data
{
    public class LocalDbService
    {
        private const string DB_NAME = "Archery_Database";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            InitializeDatabaseTablesAsync();
        }
        public async Task InitializeDatabaseTablesAsync()
        {
            await _connection.ExecuteAsync("PRAGMA foreign_keys = ON");
            await _connection.CreateTableAsync<UserAuth>();
            await _connection.CreateTableAsync<UserDetail>();
            await _connection.CreateTableAsync<UserEvents>();
            await _connection.CreateTableAsync<RoundTable>();
            await _connection.CreateTableAsync<EndTable>();
            await _connection.CreateTableAsync<ScoreItem>();
        }
        //retrieve the user credentials from the auth table.
        public async Task<UserAuth> GetUserById(int id)
        {
            return await _connection.Table<UserAuth>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        //get detail id
        public async Task<int> GetUserDetailId(int authId)
        {
            var detail = await _connection.Table<UserDetail>().Where(x => x.UserAuthId == authId).FirstOrDefaultAsync();
            return detail.Id;
        }
        //Retrieve the user by name
        public async Task<UserAuth?> GetUserByName(string username)
        {
            var auth = await _connection.Table<UserAuth>().Where(x => x.Username == username).FirstOrDefaultAsync();
            if (auth != null)
            {
                return auth;
            }
            else 
            {
                return null;
            }
        }
        //Add new user authentication data to database.
        public async Task CreateUserAuth(UserAuth auth)
        {
            await _connection.InsertAsync(auth);
        }
        //retrieve user auth id by name (username should be unique)
        public async Task<int> GetUserAuthId(string username)
        {
            var user = await _connection.Table<UserAuth>().Where(x => x.Username == username).FirstOrDefaultAsync();
            return user.Id;
        }
        //Add User Details to database
        public async Task AddUserDetailsToDatabase(User user, int userAuthId)
        {
            var u = new UserDetail
            {
                FirstName = user.ArcherName.Split(' ')[0],
                LastName = user.ArcherName.Split(' ')[1],
                Division = user.division,
                NzfaaNumber = user.NZFAANumber,
                ClubName = user.ClubName,
                ClubNumber = user.AffiliationNumber,
                DateOfBirth = user.DateOfBirth,
                UserAuthId = userAuthId
            };
            await _connection.InsertAsync(u);
        }
        //get user detail from database
        public async Task<UserDetail> GetUserDetail(int userAuthId)
        {
            var detailEntry = await _connection.Table<UserDetail>().FirstOrDefaultAsync(entry => entry.UserAuthId == userAuthId);
            return detailEntry;
        }
        //Add events to users in database
        public async Task AddEventsToUserDatabase(Event _event, int userDetailId)
        {
            var e = new UserEvents
            {
                Type = _event.Type,
                UserId = userDetailId,
                Name = _event.Name,
                Description = _event.Description,
                Date = _event.Date,
                RoundCount = _event.RoundCount,
                Environment = _event.Environment,
                Weather = _event.Weather,
                Division = _event.Division,
            };
            await _connection.InsertAsync(e);
            _event.Id = e.Id;
            await AddRoundsToDatabase(_event.Rounds, e.Id);
        }
        //retrieve each event from database for the user
        public async Task<List<Event>>GetUserEvents(int userId)
        {
            var eventTableEntries = await _connection.Table<UserEvents>().Where(e => e.UserId == userId).ToListAsync();
            if(eventTableEntries != null)
            {
                var userEvents = new List<Event>();
                foreach (var entry in eventTableEntries)
                {
                    var _event = new Event
                    {
                        Id = entry.Id,
                        Name = entry.Name,
                        Description = entry.Description,
                        Date = entry.Date,
                        RoundCount = entry.RoundCount,
                        Environment = entry.Environment,
                        Weather = entry.Weather,
                        Division = entry.Division,
                        Type = entry.Type,
                    };
                    userEvents.Add(_event);
                }
                return userEvents;
            }
            return new List<Event>();
        }

        //Add each round from app into database.
        public async Task AddRoundsToDatabase(List<Round> rounds, int eventId)
        {
            foreach (Round r in rounds)
            {
                if(r.Type == "Standard")
                {
                    var round = new RoundTable
                    {
                        Distance = r.Distance,
                        EndCount = r.EndCount,
                        Type = r.Type,
                        TargetName = r.Target.Face,
                        EventId = eventId,
                    };
                    await _connection.InsertAsync(round);
                    await AddEndsToDatabase(r.Ends, round.Id);
                }
                else
                {
                    var round = new RoundTable()
                    {
                        Distance = null,
                        TargetName = null,
                        EventId = eventId,
                        Type = r.Type,
                        EndCount = r.EndCount,
                    };
                    await _connection.InsertAsync(round);
                    await AddEndsToDatabase(r.Ends, round.Id);
                }
                
                
            }
        }
        //Retrieve each round for user from events
        public async Task<List<Round>> GetRoundsFromDatabase(int eventId)
        {
            var roundTableEntries = await _connection.Table<RoundTable>().ToListAsync();
            var filteredRounds = roundTableEntries.Where(r => r.EventId == eventId).ToList();
            if(filteredRounds != null)
            {
                var rounds = new List<Round>();
                foreach (var entry in filteredRounds)
                {
                    var round = new Round
                    {
                        Distance = entry.Distance,
                        EndCount = entry.EndCount,
                        Type = entry.Type,
                        Target = App.targets.FirstOrDefault(t => t.Face == entry.TargetName),
                        Id = entry.Id,
                    };
                    rounds.Add(round);
                }
                return rounds;
            }
            return new List<Round>();
        }

        //Add each end from app into database.
        public async Task AddEndsToDatabase(List<End> ends, int roundId)
        {
            foreach (End e in ends)
            {
                if(e.Target == null)
                {
                    var end = new EndTable
                    {
                        Distance = e.Distance,
                        ArrowCount = e.ArrowCount,
                        Number = e.EndNum,
                        TargetName = null,
                        Position = e.Position.ToString(),
                        RoundTableId = roundId
                    };
                    await _connection.InsertAsync(end);
                    e.EndId = end.Id;
                    int id = end.Id;
                    await AddScoresToDatabase(id, e.Score);
                }
                else
                {
                    var end = new EndTable
                    {
                        Distance = e.Distance,
                        Number = e.EndNum,
                        ArrowCount = e.ArrowCount,
                        TargetName = e.Target.Face,
                        Position = e.Position.ToString(),
                        RoundTableId = roundId
                    };
                    await _connection.InsertAsync(end);
                    e.EndId = end.Id;
                    int id = end.Id;
                    await AddScoresToDatabase(id, e.Score);
                }
                
            }
        }

        //Get Ends from the database
        public async Task<List<End>> RetrieveEndsFromDatabase(int roundId)
        {
            var endTableEntries = await _connection.Table<EndTable>().Where(end => end.RoundTableId == roundId).ToListAsync();
            var ends = new List<End>();
            foreach(var endTable in endTableEntries) 
            {
                var scoreItems = await _connection.Table<ScoreItem>().Where(scoreItem => scoreItem.EndId == endTable.Id).ToListAsync();
                var scores = scoreItems.Select(si => si.Score).ToList();
                var end = new End
                {
                    EndId = endTable.Id,
                    EndNum = endTable.Number,
                    Position = Enum.Parse<ShootingPosition>(endTable.Position),
                    ArrowCount = endTable.ArrowCount,
                    Distance = endTable.Distance,
                    Score = scores,
                    Target = App.targets.FirstOrDefault(t => t.Face == endTable.TargetName)
                };
                ends.Add(end);
            }
            return ends;
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

        public async Task RemoveEventFromDatabase(int eventId)
        {
            var rounds = await _connection.Table<RoundTable>().Where(r => r.EventId == eventId).ToListAsync();
            foreach(var round in rounds)
            {
                var ends = await _connection.Table<EndTable>().Where(e => e.RoundTableId == round.Id).ToListAsync();
                foreach(var end in ends)
                {
                    var scores = await _connection.Table<ScoreItem>().Where(s => s.EndId == end.Id).ToListAsync();
                    foreach(var score in scores)
                    {
                        await _connection.DeleteAsync(score);
                    }
                    await _connection.DeleteAsync(end);
                }
                await _connection.DeleteAsync(round);
            }
            var eventToRemove = await _connection.Table<UserEvents>().Where(e => e.Id == eventId).FirstOrDefaultAsync();
            if(eventToRemove != null)
            {
                await _connection.DeleteAsync(eventToRemove);
            }
        }
        public async Task<bool> CheckIfEventExists(Event _event)
        {
            var eventTableItem = await _connection.Table<UserEvents>().Where(e => e.Name == _event.Name).FirstOrDefaultAsync();
            if (eventTableItem != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task UpdateCompleteEvent(Event _event)
        {
            await UpdateEvent(_event);
            await UpdateRounds(_event.Rounds);
            foreach(Round _round in _event.Rounds)
            {
                await UpdateEnds(_round.Ends);
            }
        }
        private async Task UpdateEvent(Event _event)
        {
            var eventToUpdate = await _connection.Table<UserEvents>().Where(e => e.Id == _event.Id).FirstOrDefaultAsync();

            if (eventToUpdate != null)
            {
                eventToUpdate.Name = _event.Name;
                eventToUpdate.Description = _event.Description;
                eventToUpdate.Date = _event.Date;
                eventToUpdate.RoundCount = _event.RoundCount;
                eventToUpdate.Environment = _event.Environment;
                eventToUpdate.Weather = _event.Weather;
                eventToUpdate.Division = _event.Division;

                await _connection.UpdateAsync(eventToUpdate);
            }
        }
        private async Task UpdateRounds(List<Round> _rounds)
        {
            foreach(Round _round in _rounds)
            {
               await UpdateRound(_round);
            }
        }
        private async Task UpdateEnds(List<End> _ends)
        {
            foreach(End _end in _ends)
            {
                await UpdateEnd(_end);
                await UpdateScoreItemsForEnd(_end.EndId, _end.Score);
            }
        }
        private async Task UpdateScoreItemsForEnd(int endId, List<string> updatedScore)
        {
            var scoreItems = await _connection.Table<ScoreItem>().Where(s => s.EndId == endId).ToListAsync();
            for (int i = 0; i < scoreItems.Count; i++)
            {
                scoreItems[i].Score = updatedScore[i];
                await _connection.UpdateAsync(scoreItems[i]);
            }
        }
        private async Task UpdateRound(Round _round)
        {
            var roundToUpdate = await _connection.Table<RoundTable>().Where(r => r.Id == _round.Id).FirstOrDefaultAsync();

            if (roundToUpdate != null)
            {
                roundToUpdate.Distance = _round.Distance;
                roundToUpdate.EndCount = _round.EndCount;
                roundToUpdate.Type = _round.Type;
                roundToUpdate.TargetName = _round.Target?.Face;
                await _connection.UpdateAsync(roundToUpdate);
            }
        }
        private async Task UpdateEnd(End _end)
        {
            var endToUpdate = await _connection.Table<EndTable>().Where(e => e.Id == _end.EndId).FirstOrDefaultAsync();

            if (endToUpdate != null)
            {
                endToUpdate.Distance = _end.Distance;
                endToUpdate.ArrowCount = _end.ArrowCount;
                endToUpdate.Number = _end.EndNum;
                endToUpdate.TargetName = _end.Target?.Face;
                endToUpdate.Position = _end.Position.ToString();

                await _connection.UpdateAsync(endToUpdate);
            }
        }
    }
}
