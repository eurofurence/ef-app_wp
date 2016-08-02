using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataStore
{
    [IocBeacon(TargetType = typeof(IDataStore), Scope = IocBeacon.ScopeEnum.Singleton, Environment = IocBeacon.EnvironmentEnum.RunTimeOnly)]
    public class GzipCompressedJsonDataStore : IDataStore
    {
        private const string STORE_FILENAME_BASE = "DataStore-{0}.json.gz";
        private const string STORE_FILENAME_BLOB = "{0}-{1}.blob";


        public async Task ApplyDeltaAsync(IEnumerable<EntityBase> entities, Action<int, int, string> progressCallback = null)
        {
            var entitiesToProcess = entities as IList<EntityBase> ?? entities.ToList();
            var distinctTypes = entitiesToProcess.Select(entity => entity.GetType()).Distinct();

            int entitiesProcessed = 0;

            foreach (var distinctType in distinctTypes)
            {
                var storeEntities = await LoadAsync<EntityBase>(distinctType);
                
                foreach (var entity in entitiesToProcess.Where(entity => entity.GetType() == distinctType))
                {
                    storeEntities.RemoveAll(a => a.Id == entity.Id);
                    if (entity.IsDeleted == (byte)0)
                    {
                        storeEntities.Add(entity);
                    }
                    entitiesProcessed++;

                    progressCallback?.Invoke(entitiesProcessed, entitiesToProcess.Count, "Persisting...");
                }

                await SaveAsync(distinctType, storeEntities);
            }
        }

        private async Task<StorageFolder> GetStoreFolderAsync()
        {
            var rootFolder = ApplicationData.Current.LocalFolder;
            return await rootFolder.CreateFolderAsync("Store", CreationCollisionOption.OpenIfExists);
        }
        private async Task<StorageFolder> GetBlobStoreFolderAsync()
        {
            var rootFolder = ApplicationData.Current.LocalFolder;
            return await rootFolder.CreateFolderAsync("BlobStore", CreationCollisionOption.OpenIfExists);
        }

        private string GetFilenameForType(Type type)
        {
            return string.Format(STORE_FILENAME_BASE, type.FullName);
        }

        private string GetFilenameForBlob(Guid id, string prefix)
        {
            return string.Format(STORE_FILENAME_BLOB, prefix, id);
        }


        private async Task SaveAsync(Type entityType, IEnumerable<EntityBase> entities)
        {
            var folder = await GetStoreFolderAsync();
            var file = await folder.OpenStreamForWriteAsync(
                GetFilenameForType(entityType), CreationCollisionOption.ReplaceExisting);

            using (var gzipStream = new GZipStream(file, CompressionLevel.Optimal))
            using (var writer = new StreamWriter(gzipStream))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                var serializer = new JsonSerializer()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                var targetType = typeof (List<>).MakeGenericType(entityType);
                
                Debug.WriteLine($"Serializing type {entityType} (target: {targetType}) to {GetFilenameForType(entityType)}");

                serializer.Serialize(jsonWriter, entities.ToList(), targetType);
                jsonWriter.Flush();
            }
        }

        private async Task<List<T>> LoadAsync<T>(Type entityType = null) where T: EntityBase
        {
            var result = new List<T>();

            var folder = await GetStoreFolderAsync();
            var fileName = GetFilenameForType(entityType ?? typeof(T));
            var existingFiles = await folder.GetFilesAsync(CommonFileQuery.DefaultQuery);

            if (existingFiles.All(a => a.Name != fileName)) return result;


            var file = await folder.OpenStreamForReadAsync(fileName);

            using (var gzipStream = new GZipStream(file, CompressionMode.Decompress))
            using (var reader = new StreamReader(gzipStream))
            using (var jsonReader = new JsonTextReader(reader))
            {

                Debug.WriteLine($"Deserializing type {entityType} from {fileName}");

                var serializer = new JsonSerializer() { TypeNameHandling = TypeNameHandling.Auto };
                var f = serializer.Deserialize(jsonReader);

                if (((List<EntityBase>) f) != null)
                {
                    result.AddRange(((List<EntityBase>)f).Cast<T>());
                }
                
            }

            return result;
        }
       

        public async Task<IList<T>> GetAsync<T>() where T : EntityBase, new()
        {
            return await LoadAsync<T>();
        }

        public async Task ClearAllAsync()
        {
            var folder = await GetStoreFolderAsync();
            var files = await folder.GetFilesAsync();

            foreach (var file in files)
            {
                await file.DeleteAsync();
            }
        }

        public async Task ClearAsync(Type entityType)
        {
            var folder = await GetStoreFolderAsync();
            var file = await folder.GetFileAsync(GetFilenameForType(entityType));

            await file.DeleteAsync();
        }

        public async Task<Dictionary<string, ulong>> GetStorageFileSizesAsync()
        {
            var folder = await GetStoreFolderAsync();
            var existingFiles = await folder.GetFilesAsync(CommonFileQuery.DefaultQuery);

            var result = new Dictionary<string, ulong>();
            foreach (var file in existingFiles)
            {
                result.Add(file.Name, (await file.GetBasicPropertiesAsync()).Size);
            }

            return result;
        }

        public async Task SaveBlobAsync(Guid id, string prefix, byte[] content)
        {
            var folder = await GetBlobStoreFolderAsync();
            var file =
                await folder.OpenStreamForWriteAsync(GetFilenameForBlob(id, prefix), 
                    CreationCollisionOption.ReplaceExisting);

            await file.WriteAsync(content, 0, content.Length);
            file.Dispose();
        }

        public async Task ClearBlobAsync(Guid id, string prefix)
        {
            try
            {
                var folder = await GetBlobStoreFolderAsync();
                var fileName = GetFilenameForBlob(id, prefix);
                var file = await folder.GetFileAsync(fileName);
                await file.DeleteAsync();
            }
            catch (FileNotFoundException)
            {
            }
        }

        public async Task<byte[]> GetBlobAsync(Guid id, string prefix)
        {
            var folder = await GetBlobStoreFolderAsync();
            var fileName = GetFilenameForBlob(id, prefix);
            var file = await folder.GetFileAsync(fileName);
            var fileSize = (await file.GetBasicPropertiesAsync()).Size;
            
            var fileStream = await folder.OpenStreamForReadAsync(fileName);

            var result = new byte[fileSize];
            await fileStream.ReadAsync(result, 0, result.Length);

            return result;
        }

        public async Task<IRandomAccessStream> GetBlobStreamAsync(Guid id, string prefix)
        {
            var folder = await GetBlobStoreFolderAsync();
            var file = await folder.GetFileAsync(GetFilenameForBlob(id, prefix));
            // var fileSize = (await file.GetBasicPropertiesAsync()).Size;

            var fileStream = await file.OpenAsync(FileAccessMode.Read);
            return fileStream;
        }

        public async Task ClearAllBlobsAsync()
        {
            var folder = await GetBlobStoreFolderAsync();
            var files = await folder.GetFilesAsync();

            foreach (var file in files)
            {
                await file.DeleteAsync();
            }
        }
    }
}