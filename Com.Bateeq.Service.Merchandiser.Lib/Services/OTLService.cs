using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Com.Moonlay.NetCore.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class OTLService : BasicService<MerchandiserDbContext, OTL>, IMap<OTL, OTLViewModel>
    {
        public OTLService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        public override Tuple<List<OTL>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<OTL> Query = this.DbContext.OTLs;

            List<string> SearchAttributes = new List<string>()
                {
                    "Name", "Rate"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);
            
            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name", "Rate"
                };
            Query = Query
                .Select(b => new OTL
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    Rate = b.Rate
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<OTL> pageable = new Pageable<OTL>(Query, Page - 1, Size);
            List<OTL> Data = pageable.Data.ToList<OTL>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override void OnCreating(OTL model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }

        public OTLViewModel MapToViewModel(OTL model)
        {
            OTLViewModel viewModel = new OTLViewModel();
            PropertyCopier<OTL, OTLViewModel>.Copy(model, viewModel);
            viewModel.Rate = model.Rate;
            return viewModel;
        }

        public OTL MapToModel(OTLViewModel viewModel)
        {
            OTL model = new OTL();
            PropertyCopier<OTLViewModel, OTL>.Copy(viewModel, model);
            model.Rate = (int) viewModel.Rate;
            return model;
        }
    }
}
