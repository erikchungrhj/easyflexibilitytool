namespace EasyFlexibilityTool.Storage
{
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class AzureStorageService
    {
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _blobContainer;
        private bool _isInitialized;

        private AzureStorageService() { }

        public static async Task<AzureStorageService> GetInstanceAsync(string connectionString, string containerName)
        {
            var result = new AzureStorageService();
            await result.InitializeAsync(connectionString, containerName);

            return result;
        }

        private async Task InitializeAsync(string connectionString, string containerName)
        {
            if (!_isInitialized)
            {
                _storageAccount = CloudStorageAccount.Parse(connectionString);
                _blobClient = _storageAccount.CreateCloudBlobClient();
                _blobContainer = _blobClient.GetContainerReference(containerName);

                if (await _blobContainer.CreateIfNotExistsAsync())
                {
                    await _blobContainer.SetPermissionsAsync(new BlobContainerPermissions {PublicAccess = BlobContainerPublicAccessType.Blob});
                }

                _isInitialized = true;
            }
        }

        public async Task UploadBlobFromByteArrayAsync(string blobName, byte[] source, string contentType)
        {
            var blockBlob = _blobContainer.GetBlockBlobReference(blobName);

            await blockBlob.UploadFromByteArrayAsync(source, 0, source.Length);

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                blockBlob.Properties.ContentType = contentType;
                await blockBlob.SetPropertiesAsync();
            }
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            var blockBlob = _blobContainer.GetBlockBlobReference(blobName);
            await blockBlob.DeleteIfExistsAsync();
        }
    }
}
