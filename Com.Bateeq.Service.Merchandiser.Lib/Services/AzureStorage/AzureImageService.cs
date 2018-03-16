using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services.AzureStorage
{
    public class AzureImageService : AzureStorageService, IAzureImageService
    {
        public AzureImageService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<string> DownloadImage(string moduleName, string imageName, bool isAttachment)
        {
            return await this.DownloadImageViaSAS(moduleName, imageName, isAttachment);
        }

        private async Task<string> DownloadImageViaSAS(string moduleName, string imageName, bool isAttachment)
        {
            string uri = string.Empty;
            try
            {
                CloudBlobContainer container = this.StorageContainer;
                CloudBlobDirectory dir = container.GetDirectoryReference(moduleName);

                CloudBlockBlob blob = dir.GetBlockBlobReference(imageName);
                await blob.FetchAttributesAsync();

                //Create an ad-hoc Shared Access Policy with read permissions which will expire in 12 hours
                SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
                {
                    Permissions = SharedAccessBlobPermissions.Read,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(12),
                };
                //Set content-disposition header for force download
                SharedAccessBlobHeaders headers = new SharedAccessBlobHeaders()
                {
                    ContentDisposition = isAttachment ? string.Format("attachment;filename=\"{0}\"", imageName + "." + blob.Properties.ContentType) : string.Format("inline;filename=\"{0}\"", imageName + "." + blob.Properties.ContentType),
                };
                blob.Properties.CacheControl = "public,max-age=1000";

                string sasToken = blob.GetSharedAccessSignature(policy, headers);
                uri = blob.Uri.AbsoluteUri + sasToken;
            }
            catch (Exception ex)
            {
                if (!(ex is StorageException))
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }

            return uri;
        }

        public async Task<string> UploadImage(string moduleName, byte[] imageBytes, string imageName, string imageExtension)
        {
            return await this.UploadFromBase64(moduleName, imageBytes, imageName, imageExtension);
        }

        private async Task<string> UploadFromBase64(string moduleName, byte[] imageBytes, string imageName, string imageExtension)
        {
            string path = null;
            
            if (imageBytes != null)
            {
                CloudBlobContainer container = this.StorageContainer;
                CloudBlobDirectory dir = container.GetDirectoryReference(moduleName);

                CloudBlockBlob blob = dir.GetBlockBlobReference(imageName);
                blob.Properties.ContentType = imageExtension;
                await blob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);
                path = "/" + this.StorageContainer.Name + "/" + moduleName + "/" + imageName;
            }

            return path;
        }

        public async Task DeleteImage(string moduleName, string fileName)
        {
            CloudBlobContainer container = this.StorageContainer;
            CloudBlobDirectory dir = container.GetDirectoryReference(moduleName);

            CloudBlockBlob blob = dir.GetBlockBlobReference(fileName);
            await blob.DeleteIfExistsAsync();
        }
    }
}
