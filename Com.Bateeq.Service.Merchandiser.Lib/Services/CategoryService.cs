using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class CategoryService : BasicService<MerchandiserDbContext, Category>
    {
        public CategoryService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Category>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<Category> Query = this.DbContext.Categories;
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
                    "Id", "Code", "Name", "Description"
                };

            Query = Query
                .Select(b => new Category
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    Description = b.Description
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
            Pageable<Category> pageable = new Pageable<Category>(Query, Page - 1, Size);
            List<Category> Data = pageable.Data.ToList<Category>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        /*
        public CategoryViewModel MapToViewModel(Category Category)
        {
            CategoryViewModel CategoryVM = new CategoryViewModel();

            CategoryVM._id = Category.Id;
            CategoryVM._deleted = Category._IsDeleted;
            CategoryVM._active = Category.Active;
            CategoryVM._createdDate = Category._CreatedUtc;
            CategoryVM._createdBy = Category._CreatedBy;
            CategoryVM._createAgent = Category._CreatedAgent;
            CategoryVM._updatedDate = Category._LastModifiedUtc;
            CategoryVM._updatedBy = Category._LastModifiedBy;
            CategoryVM._updateAgent = Category._LastModifiedAgent;
            CategoryVM.code = Category.Code;
            CategoryVM.name = Category.Name;
            CategoryVM.description = Category.Description;

            return CategoryVM;
        }

        public Category MapToModel(CategoryViewModel CategoryVM)
        {
            Category Category = new Category();

            Category.Id = CategoryVM._id;
            Category._IsDeleted = CategoryVM._deleted;
            Category.Active = CategoryVM._active;
            Category._CreatedUtc = CategoryVM._createdDate;
            Category._CreatedBy = CategoryVM._createdBy;
            Category._CreatedAgent = CategoryVM._createAgent;
            Category._LastModifiedUtc = CategoryVM._updatedDate;
            Category._LastModifiedBy = CategoryVM._updatedBy;
            Category._LastModifiedAgent = CategoryVM._updateAgent;
            Category.Code = CategoryVM.code;
            Category.Name = CategoryVM.name;
            Category.Description = CategoryVM.description;

            return Category;
        }
        */
    }
}
