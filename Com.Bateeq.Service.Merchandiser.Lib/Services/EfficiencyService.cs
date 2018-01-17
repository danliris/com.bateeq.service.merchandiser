﻿using Com.Bateeq.Service.Merchandiser.Lib.Models;
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
    public class EfficiencyService : BasicService<MerchandiserDbContext, Efficiency>, IMap<Efficiency, EfficiencyViewModel>
    {
        public EfficiencyService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Efficiency>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<Efficiency> Query = this.DbContext.Efficiencies;

            List<string> SearchAttributes = new List<string>()
                {
                    "InitialRange", "FinalRange", "Value"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);
            
            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "InitialRange", "FinalRange", "Value"
                };
            Query = Query
                .Select(b => new Efficiency
                {
                    Id = b.Id,
                    Code = b.Code,
                    InitialRange = b.InitialRange,
                    FinalRange = b.FinalRange,
                    Value = b.Value
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);
            
            Pageable<Efficiency> pageable = new Pageable<Efficiency>(Query, Page - 1, Size);
            List<Efficiency> Data = pageable.Data.ToList<Efficiency>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override void OnCreating(Efficiency model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }

        public EfficiencyViewModel MapToViewModel(Efficiency model)
        {
            EfficiencyViewModel viewModel = new EfficiencyViewModel();
            PropertyCopier<Efficiency, EfficiencyViewModel>.Copy(model, viewModel);
            viewModel.InitialRange = model.InitialRange;
            viewModel.FinalRange = model.FinalRange;
            viewModel.Value = model.Value;
            return viewModel;
        }

        public Efficiency MapToModel(EfficiencyViewModel viewModel)
        {
            Efficiency model = new Efficiency();
            PropertyCopier<EfficiencyViewModel, Efficiency>.Copy(viewModel, model);
            model.InitialRange = (int)viewModel.InitialRange;
            model.FinalRange = (int)viewModel.FinalRange;
            model.Value = (int)viewModel.Value;
            return model;
        }
    }
}
