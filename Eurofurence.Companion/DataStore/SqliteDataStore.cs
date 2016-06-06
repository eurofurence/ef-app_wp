using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.DataModel;
using System.Reflection;
using Eurofurence.Companion.DataStore.Abstractions;
// ReSharper disable UnusedAutoPropertyAccessor.Local

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

        private bool _isInitialized;

        private readonly Dictionary<TypeInfo, string> _existingTypeTables = new Dictionary<TypeInfo, string>();

        private readonly SQLiteAsyncConnection _sqliteAsyncConnection;
        private readonly Assembly _localAssembly;

        public SqliteDataStore()
        {
            _localAssembly = GetType().GetTypeInfo().Assembly;
            _sqliteAsyncConnection = new SQLiteAsyncConnection(App.DatabaseStoragePath);
        }

        public async Task<IList<T>> GetAsync<T>() where T : EntityBase, new()
        {
            if (!_isInitialized) await ScanTablesAsync();

            return _existingTypeTables.ContainsKey(typeof(T).GetTypeInfo()) ?
                await _sqliteAsyncConnection.Table<T>().ToListAsync() : new List<T>();
        }

        public async Task ApplyDeltaAsync(IEnumerable<EntityBase> entities,
            Action<int, int, string> progressCallback = null)
        {
            var entitiesList = entities as IList<EntityBase> ?? entities.ToList();

            foreach (var type in entitiesList.Select(a => a.GetType()).Distinct())
            {
                await EnsureTableAsync(type.GetTypeInfo());
            }

            await _sqliteAsyncConnection.RunInTransactionAsync(connection =>
            {
                var i = 0;
                
                var total = entitiesList.Count;

                foreach (var entity in entitiesList)
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

        public async Task ClearAllAsync()
        {
            await ClearAllTablesAsync();
        }

        public async Task ClearAsync(Type entityType)
        {
            var tableName = _existingTypeTables.SingleOrDefault(a => a.Key.AsType() == entityType).Value;
            if (string.IsNullOrEmpty(tableName)) return;

            await _sqliteAsyncConnection.ExecuteAsync($"delete from {tableName};");
        }

        private async Task ClearAllTablesAsync()
        {
            foreach (var t in _existingTypeTables)
            {
                await _sqliteAsyncConnection.ExecuteAsync($"delete from {t.Value};");
            }

            await _sqliteAsyncConnection.ExecuteAsync("vacuum;");
        }


        private async Task EnsureTableAsync(TypeInfo typeInfo)
        {
            if (_existingTypeTables.ContainsKey(typeInfo)) return;
            await _sqliteAsyncConnection.CreateTablesAsync(typeInfo.AsType());
            _existingTypeTables.Add(typeInfo, typeInfo.Name);
        }

        private async Task ScanTablesAsync()
        {
            var tables = await _sqliteAsyncConnection.QueryAsync<TableInfo>
                ("SELECT * FROM sqlite_master WHERE type = 'table'");

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

            _isInitialized = true;
        }
    }
}