using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using System.Threading.Tasks;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class CategoryService : BasicService<MerchandiserDbContext, Category>, IMap<Category, CategoryViewModel>
    {
        public CategoryService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Category>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<Category> Query = this.DbContext.Categories;

            List<string> SearchAttributes = new List<string>()
                {
                    "Name", "SubCategory"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);
            
            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name", "SubCategory"
                };
            Query = Query
                .Select(b => new Category
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    SubCategory = b.SubCategory
                });

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);
            
            Pageable<Category> pageable = new Pageable<Category>(Query, Page - 1, Size);
            List<Category> Data = pageable.Data.ToList<Category>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override async Task<int> DeleteModel(int Id)
        {
            MaterialService materialService = this.ServiceProvider.GetService<MaterialService>();

            int deleted = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    deleted = await this.DeleteAsync(Id);

                    HashSet<int> deletedMaterials = new HashSet<int>(materialService.DbSet
                        .Where(p => p.CategoryId.Equals(Id))
                        .Select(p => p.Id));
                    foreach (int deletedMaterial in deletedMaterials)
                    {
                        await materialService.DeleteModel(deletedMaterial);
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return deleted;
        }

        public override void OnCreating(Category model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }

        public CategoryViewModel MapToViewModel(Category model)
        {
            CategoryViewModel viewModel = new CategoryViewModel();
            PropertyCopier<Category, CategoryViewModel>.Copy(model, viewModel);
            return viewModel;
        }

        public Category MapToModel(CategoryViewModel viewModel)
        {
            Category model = new Category();
            PropertyCopier<CategoryViewModel, Category>.Copy(viewModel, model);
            return model;
        }
    }
}
