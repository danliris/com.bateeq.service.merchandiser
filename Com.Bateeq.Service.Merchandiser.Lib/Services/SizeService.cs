using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using System.Threading.Tasks;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class SizeService : BasicService<MerchandiserDbContext, Size>, IMap<Size, SizeViewModel>
    {
        public SizeService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Size>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<Size> Query = this.DbContext.Sizes;

            List<string> SearchAttributes = new List<string>()
                {
                    "Code", "Name"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name"
                };
            Query = Query
                .Select(b => new Size
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<Size> pageable = new Pageable<Size>(Query, Page - 1, Size);
            List<Size> Data = pageable.Data.ToList<Size>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override async Task<int> DeleteModel(int Id)
        {
            RelatedSizeService relatedSizeService = this.ServiceProvider.GetService<RelatedSizeService>();

            int deleted = await this.DeleteAsync(Id);
            HashSet<int> deletedRelatedSizes = new HashSet<int>(relatedSizeService.DbSet
                .Where(p => p.SizeRangeId.Equals(Id))
                .Select(p => p.Id));

            foreach (int relatedSize in deletedRelatedSizes)
            {
                await relatedSizeService.DeleteModel(relatedSize);
            }

            return deleted;
        }

        public override void OnCreating(Size model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }

        public SizeViewModel MapToViewModel(Size model)
        {
            SizeViewModel viewModel = new SizeViewModel();
            PropertyCopier<Size, SizeViewModel>.Copy(model, viewModel);
            return viewModel;
        }

        public Size MapToModel(SizeViewModel viewModel)
        {
            Size model = new Size();
            PropertyCopier<SizeViewModel, Size>.Copy(viewModel, model);
            return model;
        }
    }
}
