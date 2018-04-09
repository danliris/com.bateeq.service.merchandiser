using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public static class TimestampGenerator
    {
        private const string TIMESTAMP_FORMAT = "yyyyMMddHHmmssffff";
        public static string GenerateTimestamp(DateTime value)
        {
            return value.ToString(TIMESTAMP_FORMAT);
        }
    }
}
