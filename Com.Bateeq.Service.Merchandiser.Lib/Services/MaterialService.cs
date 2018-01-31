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
using Microsoft.EntityFrameworkCore;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class MaterialService : BasicService<MerchandiserDbContext, Material>, IMap<Material, MaterialViewModel>
    {
        public MaterialService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Material>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<Material> Query = this.DbContext.Materials;

            List<string> SearchAttributes = new List<string>()
                {
                    "Code", "Name"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name", "Description", "Category"
                };
            Query = Query
                .Select(m => new Material
                {
                    Id = m.Id,
                    Code = m.Code,
                    Name = m.Name,
                    Description = m.Description,
                    Category = new Category
                    {
                        Id = m.Category.Id,
                        Name = m.Category.Name,
                        SubCategory = m.Category.SubCategory
                    },
                    CategoryId = m.CategoryId
                });

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<Material> pageable = new Pageable<Material>(Query, Page - 1, Size);
            List<Material> Data = pageable.Data.ToList<Material>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public async Task<List<Material>> ReadModelOnCategory(int Id)
        {
            return await this.DbSet
                .Where(m => m.CategoryId.Equals(Id) && m._IsDeleted == false)
                .ToListAsync();
        }

        public override async Task<Material> ReadModelById(int Id)
        {
            return await this.DbSet
                .Where(m => m.Id.Equals(Id) && m._IsDeleted == false)
                .Include(m => m.Category)
                .FirstOrDefaultAsync();
        }

        public override void OnCreating(Material model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }

        public MaterialViewModel MapToViewModel(Material model)
        {
            MaterialViewModel viewModel = new MaterialViewModel();

            PropertyCopier<Material, MaterialViewModel>.Copy(model, viewModel);
            MaterialViewModel.CategoryVM categoryVM = new MaterialViewModel.CategoryVM();
            PropertyCopier<Category, MaterialViewModel.CategoryVM>.Copy(model.Category, categoryVM);
            viewModel.Category = categoryVM;

            return viewModel;
        }

        public Material MapToModel(MaterialViewModel viewModel)
        {
            Material model = new Material();

            PropertyCopier<MaterialViewModel, Material>.Copy(viewModel, model);
            model.CategoryId = viewModel.Category.Id;

            return model;
        }
    }
}