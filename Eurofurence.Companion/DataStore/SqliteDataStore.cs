using Eurofurence.Companion.DataModel.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurofurence.Companion.DataStore
{
    public class SqliteDataStore : IDataStore
    {
        private SQLite.SQLiteAsyncConnection _sqliteAsyncConnection;

        private List<Type> _entityTypes = new List<Type>() {
            typeof(EventEntry),
            typeof(EventConferenceDay),
            typeof(EventConferenceRoom),
            typeof(EventConferenceTrack),
            typeof(Info),
            typeof(InfoGroup),
            typeof(Image)
        };

        public SqliteDataStore()
        {
            _sqliteAsyncConnection = new SQLite.SQLiteAsyncConnection(App.DatabaseStoragePath);
            CreateTablesAsync().Wait();
        }

        private async Task TruncateTablesAsync()
        {
            foreach(var t in _entityTypes)
            {
                await _sqliteAsyncConnection.ExecuteAsync($"delete from {t.Name};");
            }

            await _sqliteAsyncConnection.ExecuteAsync("vacuum;");
        }

        private Task CreateTablesAsync()
        {
            return _sqliteAsyncConnection.CreateTablesAsync(_entityTypes.ToArray());
        }

        public async Task<IList<T>> GetAsync<T>() where T: EntityBase, new()
        {
            return await _sqliteAsyncConnection.Table<T>().ToListAsync();
        }

        public async Task ApplyDeltaAsync(IEnumerable<EntityBase> entities,
            Action<int, int, string> progressCallback = null)
        {
            await _sqliteAsyncConnection.RunInTransactionAsync((connection) =>
            {
                int i = 0;
                int total = entities.Count();

                foreach (var entity in entities)
                {
                    if (entity.IsDeleted == 1)
                    {
                        connection.Delete(entity);
                    }
                    else
                    {
                        var obj = connection.Find(entity.Id, new SQLite.TableMapping(entity.GetType()));
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
    }
}

