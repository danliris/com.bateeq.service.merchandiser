using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class SizeRangeService : BasicService<MerchandiserDbContext, SizeRange>, IMap<SizeRange, SizeRangeViewModel>
    {
        public SizeRangeService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<SizeRange>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<SizeRange> Query = this.DbContext.SizeRanges;

            List<string> SearchAttributes = new List<string>()
                {
                    "Code", "Name"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);
            
            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name", "RelatedSizes"
                };
            Query = Query
                .Select(sr => new SizeRange
                {
                    Id = sr.Id,
                    Code = sr.Code,
                    Name = sr.Name,
                    RelatedSizes = sr.RelatedSizes
                        .Select(rs => new RelatedSize
                        {
                            Id = rs.Id,
                            Size = new Size
                            { 
                                Id = rs.Size.Id,
                                Code = rs.Size.Code,
                                Name = rs.Size.Name
                            }
                        })
                        .ToList()
                });

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);
            
            Pageable<SizeRange> pageable = new Pageable<SizeRange>(Query, Page - 1, Size);
            List<SizeRange> Data = pageable.Data.ToList<SizeRange>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }
        
        public override async Task<SizeRange> ReadModelById(int id)
        {
            return await this.DbSet
                .Where(d => d.Id.Equals(id))
                .Include(sr => sr.RelatedSizes)
                    .ThenInclude(rs => rs.Size)
                .FirstOrDefaultAsync();
        }

        public override async Task<int> UpdateModel(int Id, SizeRange Model)
        {
            RelatedSizeService relatedSizeService = this.ServiceProvider.GetService<RelatedSizeService>();
            
            int updated = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    HashSet<int> relatedSizes = new HashSet<int>(relatedSizeService.DbSet
                        .Where(p => p.SizeRangeId.Equals(Id))
                        .Select(p => p.Id));
                    updated = await this.UpdateAsync(Id, Model);

                    foreach (int relatedSize in relatedSizes)
                    {
                        RelatedSize rs = Model.RelatedSizes.FirstOrDefault(prop => prop.Id.Equals(relatedSize));

                        if (rs == null)
                        {
                            await relatedSizeService.DeleteModel(relatedSize);
                        }
                    }

                    foreach (RelatedSize relatedSize in Model.RelatedSizes)
                    {
                        if (relatedSize.Id.Equals(0))
                        {
                            await relatedSizeService.CreateModel(relatedSize);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return updated;
        }

        public override async Task<int> DeleteModel(int Id)
        {
            RelatedSizeService relatedSizeService = this.ServiceProvider.GetService<RelatedSizeService>();

            int deleted = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    deleted = await this.DeleteAsync(Id);

                    HashSet<int> deletedRelatedSizes = new HashSet<int>(relatedSizeService.DbSet
                        .Where(p => p.SizeRangeId.Equals(Id))
                        .Select(p => p.Id));
                    foreach (int relatedSize in deletedRelatedSizes)
                    {
                        await relatedSizeService.DeleteModel(relatedSize);
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

        public override void OnCreating(SizeRange model)
        {
            do
            {
                model.Code = CodeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(sr => sr.Code.Equals(model.Code)));

            if (model.RelatedSizes.Count > 0)
            {
                RelatedSizeService relatedSizeService = this.ServiceProvider.GetService<RelatedSizeService>();
                foreach (RelatedSize relatedSize in model.RelatedSizes)
                {
                    relatedSizeService.OnCreating(relatedSize);
                }
            }

            base.OnCreating(model);
        }

        public SizeRangeViewModel MapToViewModel(SizeRange model)
        {
            SizeRangeViewModel viewModel = new SizeRangeViewModel();
            viewModel.RelatedSizes = new List<SizeRangeViewModel.RelatedSizeVM>();
            PropertyCopier<SizeRange, SizeRangeViewModel>.Copy(model, viewModel);
            SizeService sizeService = this.ServiceProvider.GetService<SizeService>();
            foreach (RelatedSize relatedSize in model.RelatedSizes)
            {
                SizeRangeViewModel.RelatedSizeVM relatedSizeVM = new SizeRangeViewModel.RelatedSizeVM();
                PropertyCopier<RelatedSize, SizeRangeViewModel.RelatedSizeVM>.Copy(relatedSize, relatedSizeVM);
                SizeRangeViewModel.RelatedSizeVM.SizeVM sizeVM = new SizeRangeViewModel.RelatedSizeVM.SizeVM();
                sizeVM.Id = relatedSize.Size.Id;
                sizeVM.Code = relatedSize.Size.Code;
                sizeVM.Name = relatedSize.Size.Name;
                relatedSizeVM.Size = sizeVM;
                viewModel.RelatedSizes.Add(relatedSizeVM);
            }
            return viewModel;
        }

        public SizeRange MapToModel(SizeRangeViewModel viewModel)
        {
            SizeRange model = new SizeRange();
            model.RelatedSizes = new List<RelatedSize>();
            PropertyCopier<SizeRangeViewModel, SizeRange>.Copy(viewModel, model);
            foreach (SizeRangeViewModel.RelatedSizeVM relatedSizeVM in viewModel.RelatedSizes)
            {
                RelatedSize relatedSize = new RelatedSize();
                PropertyCopier<SizeRangeViewModel.RelatedSizeVM, RelatedSize>.Copy(relatedSizeVM, relatedSize);
                relatedSize.SizeId = relatedSizeVM.Size.Id;
                model.RelatedSizes.Add(relatedSize);
            }
            return model;
        }
    }
}
