using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Exceptions
{
    public class DbReferenceNotNullException : Exception
    {
        public DbReferenceNotNullException()
        {
        }

        public DbReferenceNotNullException(string message) : base(message)
        {
        }
    }
}
