using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public interface IGeneralService<TModel, TViewModel>
    {
        Tuple<List<TModel>, int, Dictionary<string, string>, List<string>> Read(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null);

        TViewModel MapToViewModel(TModel model);

        TModel MapToModel(TViewModel viewModel);
    }
}
