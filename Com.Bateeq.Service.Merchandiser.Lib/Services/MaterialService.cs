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
    public class MaterialService : BasicService<MerchandiserDbContext, Material>
    {
        public MaterialService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Material>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<Material> Query = this.DbContext.Materials;
            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            // Search With Keyword
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                    {
                        "Code", "Name"
                    };

                Query = Query.Where(General.BuildSearch(SearchAttributes, Keyword), Keyword);
            }

            // Const Select
            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name", "Description", "Category", "CategoryId"
                };

            Query = Query
                .Select(m => new Material
                {
                    Id = m.Id,
                    Code = m.Code,
                    Name = m.Name,
                    Description = m.Description,
                    Category = m.Category,
                    CategoryId = m.CategoryId
                });

            // Order
            if (OrderDictionary.Count.Equals(0))
            {
                OrderDictionary.Add("_LastModifiedUtc", General.DESCENDING);

                Query = Query.OrderByDescending(b => b._LastModifiedUtc); // Default Order
            }
            else
            {
                string Key = OrderDictionary.Keys.First();
                string OrderType = OrderDictionary[Key];
                string TransformKey = General.TransformOrderBy(Key);

                BindingFlags IgnoreCase = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

                Query = OrderType.Equals(General.ASCENDING) ?
                    Query.OrderBy(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b)) :
                    Query.OrderByDescending(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b));
            }

            // Pagination
            Pageable<Material> pageable = new Pageable<Material>(Query, Page - 1, Size);
            List<Material> Data = pageable.Data.ToList<Material>();
            int TotalData = pageable.TotalCount;

            //IQueryable<Category> categoryQuery = this.DbContext.Categories;
            //foreach (Material material in Data)
            //{
            //    var category = categoryQuery
            //        .Select(c => new Category
            //        {
            //            Id = c.Id,
            //            Code = c.Code,
            //            Name = c.Name,
            //            SubCategory = c.SubCategory
            //        })
            //        .Where(c => c.Id == material.CategoryId)
            //        .FirstOrDefault();
            //    material.Category = (Category) category;
            //}

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override async Task<int> CreateModel(Material Model)
        {
            if (Model.Category != null)
                Model.CategoryId = Model.Category.Id;
            Model.Category = null;

            return await this.CreateAsync(Model);
        }

        public override async Task<Material> ReadModelById(int Id)
        {
            var model = await this.GetAsync(Id);

            CategoryService categoryService = this.ServiceProvider.GetService<CategoryService>();
            model.Category = await categoryService.ReadModelById(model.CategoryId);
            model.Category.Materials = null;

            return model;
        }

        public override async Task<int> UpdateModel(int Id, Material Model)
        {
            if (Model.Category != null)
                Model.CategoryId = Model.Category.Id;
            Model.Category = null;

            return await this.UpdateAsync(Id, Model);
        }

        public override void OnCreating(Material model)
        {
            CodeGenerator codeGenerator = new CodeGenerator();

            do
            {
                model.Code = codeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }
    }
}