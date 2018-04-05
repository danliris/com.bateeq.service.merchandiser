namespace Com.Bateeq.Service.Merchandiser.Lib.Interfaces
{
    public interface IMap<TModel, TViewModel>
    {
        TViewModel MapToViewModel(TModel model);
        TModel MapToModel(TViewModel viewModel);
    }
}