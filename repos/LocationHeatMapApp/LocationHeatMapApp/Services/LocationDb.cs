using SQLite;
using LocationHeatMapApp.Models;

namespace LocationHeatMapApp.Services;

public class LocationDb
{
    private readonly SQLiteAsyncConnection _db;

    public LocationDb(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
    }

    public Task InitAsync() =>
        _db.CreateTableAsync<LocationPoint>();

    public Task<int> InsertAsync(LocationPoint p) =>
        _db.InsertAsync(p);

    public Task<List<LocationPoint>> GetRecentAsync(int take = 600) =>
        _db.Table<LocationPoint>()
           .OrderByDescending(x => x.TimestampUtc)
           .Take(take)
           .ToListAsync();

    public Task<int> ClearAsync() =>
        _db.DeleteAllAsync<LocationPoint>();
}
