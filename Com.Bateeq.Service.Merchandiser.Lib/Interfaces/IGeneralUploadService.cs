using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Interfaces
{
    public interface IGeneralUploadService<TViewModel>
    {
        Tuple<bool, List<object>> UploadValidate(List<TViewModel> Data);
        List<string> CsvHeader { get; }
    }
}
