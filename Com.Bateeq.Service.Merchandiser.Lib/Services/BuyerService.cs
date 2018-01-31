using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class BuyerService : BasicService<MerchandiserDbContext, Buyer>, IMap<Buyer, BuyerViewModel>
    {
        public BuyerService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Buyer>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<Buyer> Query = this.DbContext.Buyers;

            List<string> SearchAttributes = new List<string>()
                {
                    "Name", "Email"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);
            
            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name", "Email", "Address1", "Address2"
                };
            Query = Query
                .Select(b => new Buyer
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    Email = b.Email,
                    Address1 = b.Address1,
                    Address2 = b.Address2
                });

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<Buyer> pageable = new Pageable<Buyer>(Query, Page - 1, Size);
            List<Buyer> Data = pageable.Data.ToList<Buyer>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override void OnCreating(Buyer model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }

        public BuyerViewModel MapToViewModel(Buyer model)
        {
            BuyerViewModel viewModel = new BuyerViewModel();
            PropertyCopier<Buyer, BuyerViewModel>.Copy(model, viewModel);
            return viewModel;
        }

        public Buyer MapToModel(BuyerViewModel viewModel)
        {
            Buyer model = new Buyer();
            PropertyCopier<BuyerViewModel, Buyer>.Copy(viewModel, model);
            return model;
        }
    }
}
