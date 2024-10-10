﻿using SQLite;
using ArcheryLibrary;

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
        }
        //retrieve the user credentials from the auth table.
        public async Task<UserAuth> GetUserById(int id)
        {
            return await _connection.Table<UserAuth>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        //Add new user authentication data to database.
        public async Task CreateUserAuth(UserAuth auth)
        {
            await _connection.InsertAsync(auth);
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
        }
        //retrieve each event from database for the user
        public async Task<List<Event>>GetUserEvents(int userId)
        {
            var eventTableEntries = await _connection.Table<UserEvents>().Where(e => e.UserId == userId).ToListAsync();
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
                userEvents.Add( _event );
            }
            return userEvents;
        }

        //Add each round from app into database.
        public async Task AddRoundsToDatabase(List<Round> rounds, int eventId)
        {
            foreach (Round r in rounds)
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
            }
        }
        //Retrieve each round for user from events
        public async Task<List<Round>> GetRoundsFromDatabase(int eventId)
        {
            var roundTableEntries = await _connection.Table<RoundTable>().Where(r => r.EventId == eventId).ToListAsync();
            var rounds = new List<Round>();
            foreach(var entry in roundTableEntries)
            {
                var round = new Round
                {
                    Distance = entry.Distance,
                    EndCount = entry.EndCount,
                    Type = entry.Type,
                    Target = App.targets.FirstOrDefault(t => t.Face == entry.TargetName),

                };
                rounds.Add( round );
            }
            return rounds;
        }

        //Add each end from app into database.
        public async Task AddEndsToDatabase(List<End> ends, int roundId)
        {
            foreach (End e in ends)
            {
                var end = new EndTable
                {
                    Distance = e.Distance,
                    Number = e.EndNum,
                    TargetName = e.Target.Face,
                    Position = e.Position.ToString(),
                    RoundTableId = roundId
                };
                await _connection.InsertAsync(end);
            }
        }

        //Get Ends from the database
        public async Task<List<End>> RetrieveEndsFromDatabase(int roundId)
        {
            var endTableEntries = _connection.Table<EndTable>().Where(end => end.RoundTableId == roundId).ToListAsync();
            var ends = new List<End>();
            foreach(var endTable in await endTableEntries) 
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
    }
}
