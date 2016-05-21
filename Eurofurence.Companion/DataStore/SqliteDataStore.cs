using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eurofurence.Companion.DataModel.Api;
using SQLite;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.DataStore
{
    [IocBeacon(TargetType = typeof(IDataStore), Scope = IocBeacon.ScopeEnum.Singleton, Environment = IocBeacon.EnvironmentEnum.RunTimeOnly)]
    public class SqliteDataStore : IDataStore
    {
        private readonly List<Type> _entityTypes = new List<Type>
        {
            typeof (EventEntry),
            typeof (EventConferenceDay),
            typeof (EventConferenceRoom),
            typeof (EventConferenceTrack),
            typeof (Info),
            typeof (InfoGroup),
            typeof (Image),
            typeof (Dealer)
        };

        private readonly SQLiteAsyncConnection _sqliteAsyncConnection;

        public SqliteDataStore()
        {
            _sqliteAsyncConnection = new SQLiteAsyncConnection(App.DatabaseStoragePath);
            CreateTablesAsync().Wait();
        }

        public async Task<IList<T>> GetAsync<T>() where T : EntityBase, new()
        {
            return await _sqliteAsyncConnection.Table<T>().ToListAsync();
        }

        public async Task ApplyDeltaAsync(IEnumerable<EntityBase> entities,
            Action<int, int, string> progressCallback = null)
        {
            await _sqliteAsyncConnection.RunInTransactionAsync(connection =>
            {
                var i = 0;
                var total = entities.Count();

                foreach (var entity in entities)
                {
                    if (entity.IsDeleted == 1)
                    {
                        connection.Delete(entity);
                    }
                    else
                    {
                        var obj = connection.Find(entity.Id, new TableMapping(entity.GetType()));
                        if (obj != null)
                        {
                            connection.Update(entity);
                        }
                        else
                        {
                            connection.Insert(entity);
                        }
                    }
                    progressCallback?.Invoke(++i, total, entity.Id.ToString());
                }

                connection.Commit();
            });
        }

        public Task ClearAllAsync()
        {
            return TruncateTablesAsync();
        }

        private async Task TruncateTablesAsync()
        {
            foreach (var t in _entityTypes)
            {
                await _sqliteAsyncConnection.ExecuteAsync($"delete from {t.Name};");
            }

            await _sqliteAsyncConnection.ExecuteAsync("vacuum;");
        }

        private Task CreateTablesAsync()
        {
            return _sqliteAsyncConnection.CreateTablesAsync(_entityTypes.ToArray());
        }
    }
}