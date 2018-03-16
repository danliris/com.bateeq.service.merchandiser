using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public static class ImageHelper
    {
        public static string GetFileNameFromPath(string imagePath)
        {
            string[] filePath = imagePath.Split('/');
            return filePath[filePath.Length - 1];
        }

        public static string GenerateFileName(int id, DateTime _createdUtc)
        {
            return String.Format("IMG_{0}_{1}", id, TimestampGenerator.GenerateTimestamp(_createdUtc));
        }

        public static string GenerateFileName(int id, DateTime _createdUtc, int index)
        {
            return String.Format("IMG_{0}_{1}_{2}", id, index, TimestampGenerator.GenerateTimestamp(_createdUtc));
        }

        public static byte[] ConvertFromBase64String(string imageBase64)
        {
            byte[] imageBytes = null;
            if (imageBase64 != null)
            {
                try
                {
                    imageBytes = Convert.FromBase64String(imageBase64);
                }
                catch (Exception ex)
                {
                    if (!(ex is ArgumentNullException) && !(ex is FormatException))
                    {
                        throw new Exception(ex.Message, ex.InnerException);
                    }
                }
            }
            return imageBytes;
        }
    }
}
