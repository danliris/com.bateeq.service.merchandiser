using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Lib.Interfaces
{
    public interface IAzureImageService
    {
        Task<string> DownloadImage(string moduleName, string imageName, bool isAttachment);
        Task<string> UploadImage(string moduleName, byte[] imageBytes, string imageName, string imageExtension);
    }
}
