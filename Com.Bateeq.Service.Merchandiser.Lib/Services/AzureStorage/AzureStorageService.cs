using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services.AzureStorage
{
    public class AzureStorageService
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        protected CloudStorageAccount StorageAccount { get; private set; }
        protected CloudBlobContainer StorageContainer { get; private set; }

        public AzureStorageService(IServiceProvider serviceProvider)
        {
            string storageAccountName = Environment.GetEnvironmentVariable("StorageAccountName");
            string storageAccountKey = Environment.GetEnvironmentVariable("StorageAccountKey");
            string storageContainer = "merchandiser";

            this.ServiceProvider = serviceProvider;
            this.StorageAccount = new CloudStorageAccount(new StorageCredentials(storageAccountName, storageAccountKey), useHttps: true);
            this.StorageContainer = this.Configure(storageContainer).GetAwaiter().GetResult();
        }

        private async Task<CloudBlobContainer> Configure(string storageContainer)
        {
            CloudBlobClient cloudBlobClient = this.StorageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(storageContainer);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            BlobContainerPermissions permissions = SetContainerPermission(true);
            await cloudBlobContainer.SetPermissionsAsync(permissions);

            return cloudBlobContainer;
        }

        private BlobContainerPermissions SetContainerPermission(Boolean isPublic)
        {
            BlobContainerPermissions permissions = new BlobContainerPermissions();
            if (isPublic)
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            else
                permissions.PublicAccess = BlobContainerPublicAccessType.Off;
            return permissions;
        }
    }
}
