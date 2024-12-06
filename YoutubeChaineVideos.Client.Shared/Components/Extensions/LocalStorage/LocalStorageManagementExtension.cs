using Blazored.LocalStorage;
using System.Text;
using System.Text.Json;

namespace YoutubeChaineVideos.Client.Shared.Components.Extensions.LocalStorage
{
    public static class LocalStorageManagementExtension
    {
        public static async Task SetItemEncryptedAsync<TEntityModelView>(this ILocalStorageService localStorageService, string key, TEntityModelView entity)
        {
            var entityJson = JsonSerializer.Serialize(entity);
            var entityJsonByte = Encoding.UTF8.GetBytes(entityJson);
            var entityJsonBase64 = Convert.ToBase64String(entityJsonByte);
            await localStorageService.SetItemAsync(key, entityJsonBase64);
        }

        public static async Task<TEntityModelView?> GetItemDecryptedAsync<TEntityModelView>(this ILocalStorageService localStorageService, string key)
        {
            if (key == null) return default;
            var entityJsonBase64 = await localStorageService.GetItemAsync<string>(key);
            if (entityJsonBase64 == null) return default;
            var entityJsonByte = Convert.FromBase64String(entityJsonBase64);
            var entityJson = Encoding.UTF8.GetString(entityJsonByte);
            var entity = JsonSerializer.Deserialize<TEntityModelView>(entityJson);
            return entity;
        }
    }
}
