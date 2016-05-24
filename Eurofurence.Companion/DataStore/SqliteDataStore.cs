using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.DataModel;
using System.Reflection;

namespace Eurofurence.Companion.DataStore
{
    [IocBeacon(TargetType = typeof(IDataStore), Scope = IocBeacon.ScopeEnum.Singleton, Environment = IocBeacon.EnvironmentEnum.RunTimeOnly)]
    public class SqliteDataStore : IDataStore
    {
        private class TableInfo
        {
            [Column("name")]
            public string Name { get; set; }
        }

        private readonly Dictionary<TypeInfo, string> _existingTypeTables = new Dictionary<TypeInfo, string>();

        private readonly SQLiteAsyncConnection _sqliteAsyncConnection;
        private Assembly _localAssembly;

        public SqliteDataStore()
        {
            _localAssembly = GetType().GetTypeInfo().Assembly;
            _sqliteAsyncConnection = new SQLiteAsyncConnection(App.DatabaseStoragePath);

            ScanTablesAsync().Wait();
        }

        public async Task<IList<T>> GetAsync<T>() where T : EntityBase, new()
        {
            return _existingTypeTables.ContainsKey(typeof(T).GetTypeInfo()) ?
                await _sqliteAsyncConnection.Table<T>().ToListAsync() : new List<T>();
        }

        public async Task ApplyDeltaAsync(IEnumerable<EntityBase> entities,
            Action<int, int, string> progressCallback = null)
        {
            await _sqliteAsyncConnection.RunInTransactionAsync(async (connection) =>
            {
                var i = 0;
                var total = entities.Count();

                foreach (var entity in entities)
                {
                    await EnsureTableAsync(entity.GetType().GetTypeInfo()).ConfigureAwait(false);

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

        public async Task ClearAllAsync()
        {
            await ClearAllTablesAsync().ConfigureAwait(false);
        }

        private async Task ClearAllTablesAsync()
        {
            foreach (var t in _existingTypeTables)
            {
                await _sqliteAsyncConnection.ExecuteAsync($"delete from {t.Value};")
                    .ConfigureAwait(false);
            }

            await _sqliteAsyncConnection.ExecuteAsync("vacuum;")
                .ConfigureAwait(false); 
        }


        private async Task EnsureTableAsync(TypeInfo typeInfo)
        {
            if (_existingTypeTables.ContainsKey(typeInfo)) return;
            await _sqliteAsyncConnection.CreateTablesAsync(typeInfo.AsType()).ConfigureAwait(false);
            _existingTypeTables.Add(typeInfo, typeInfo.Name);
        }

        private async Task ScanTablesAsync()
        {
            var tables = await _sqliteAsyncConnection.QueryAsync<TableInfo>
                ("SELECT * FROM sqlite_master WHERE type = 'table'")
                .ConfigureAwait(false);

            var _localAssemblyEntityTypes = _localAssembly.DefinedTypes
                .Where(typeInfo => typeof(EntityBase).GetTypeInfo().IsAssignableFrom(typeInfo)).ToList();

            foreach (var table in tables)
            {
                var localEntityType = _localAssemblyEntityTypes.SingleOrDefault(typeInfo => typeInfo.Name == table.Name);
                System.Diagnostics.Debug.WriteLine($"{table.Name} -> {localEntityType}");
                if (localEntityType != null)
                {
                    _existingTypeTables.Add(localEntityType, table.Name);
                }
            }
        }
    }
}