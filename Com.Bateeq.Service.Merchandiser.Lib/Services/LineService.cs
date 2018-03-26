using Com.Bateeq.Service.Merchandiser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System.Linq.Dynamic.Core;
using Com.Moonlay.NetCore.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;

namespace Com.Bateeq.Service.Merchandiser.Lib.Services
{
    public class LineService : BasicService<MerchandiserDbContext, Line>, IMap<Line, LineViewModel>
    {
        public LineService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Line>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<Line> Query = this.DbContext.Lines;

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
                .Select(b => new Line
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<Line> pageable = new Pageable<Line>(Query, Page - 1, Size);
            List<Line> Data = pageable.Data.ToList<Line>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public LineViewModel MapToViewModel(Line model)
        {
            LineViewModel viewModel = new LineViewModel();
            PropertyCopier<Line, LineViewModel>.Copy(model, viewModel);
            return viewModel;
        }

        public Line MapToModel(LineViewModel viewModel)
        {
            Line model = new Line();
            PropertyCopier<LineViewModel, Line>.Copy(viewModel, model);
            return model;
        }
    }
}
