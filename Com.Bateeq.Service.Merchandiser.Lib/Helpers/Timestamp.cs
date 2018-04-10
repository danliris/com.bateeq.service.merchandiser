using System;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public static class Timestamp
    {
        private const string TIMESTAMP_FORMAT = "yyyyMMddHHmmssffff";
        public static string Generate(DateTime value)
        {
            return value.ToString(TIMESTAMP_FORMAT);
        }
    }
}
